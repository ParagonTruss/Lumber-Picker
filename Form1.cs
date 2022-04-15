using Ookii.Dialogs.WinForms;
using ParagonApi;
using ParagonApi.Files;
using ParagonApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lumber_Picker
{
    public partial class Form1 : Form
    {
        public static string StationName = "Floor Station";
        public static List<Chord> Chords = new List<Chord>();
        //public static List<Member> Chords = new List<Member>();
        public Form1()
        {
            InitializeComponent();
        }

        private async void LoadButton_Click(object sender, EventArgs e)
        {

            var fbd = new VistaFolderBrowserDialog();
            var result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {

                var path = fbd.SelectedPath;
                string[] files = Directory.GetFiles(path);
                using (var connection = await Paragon.Connect(ParagonApi.Environments.Environment.Production))
                {



                    var stationGuid = (await connection.Stations.FindByName(StationName)).Guid; // we don't hold onto the station itself because we need the updated componentDesigns

                    var batchPath = path + @"\\..\\";
                    var cntFiles = Directory.GetFiles(batchPath, "*.cnt");

                    if (cntFiles.Count() != 1)
                    {
                        MessageBox.Show($"There Should be a Single .cnt file at {batchPath}");
                        return;
                    }
                    var cntPath = cntFiles.First();

                    await connection.Stations.Clear(stationGuid);

                    foreach (var trussPath in files)
                    {
                        if (!trussPath.Contains(".tre"))
                        {
                            continue;
                        }

                        var uploadFile = BinaryUploadFile.Create(trussPath);
                        var uploadedTruss = await connection.Trusses.Upload(uploadFile);
                        foreach (var trussGuid in uploadedTruss.ComponentDesignGuids)
                        {
                            await connection.Stations.AddComponentDesign(stationGuid, trussGuid);
                        }
                    }

                    var cntFile = BinaryUploadFile.Create(cntPath);
                    await connection.Stations.UploadCountFile(stationGuid, cntFile);

                    //Part 2
                    var station = await connection.Stations.FindByName(StationName);

                    var componentDesigns = station.ComponentDesigns;//.OrderBy(design => design.Value.Index).ToList();

                    var trusses = new List<(Truss Truss, int Quantity)>();
                    foreach (var componentDesign in componentDesigns)
                    {
                        var truss = await connection.Trusses.Find(componentDesign.Key);
                        var quantity = componentDesign.Value.Quantity.Value;
                        trusses.Add((truss, quantity));
                    }

                    trusses = trusses.OrderByDescending(x => x.Truss.Width)
                                     .ThenByDescending(x => x.Truss.Name).ToList();

                    foreach (var truss in trusses)
                    {
                        var totalQuantity = truss.Quantity * truss.Truss.Plies;
                        for (int i = 0; i < totalQuantity; i++)
                        {
                            var paragonChords = truss.Truss.Members.Where(member => member.Type == "TopChord"|| member.Type== "BottomChord");
                            var orderedChords = paragonChords.OrderByDescending(chord => chord.Geometry.Max(Point => Point.Y))
                                                             .ThenBy(chord => chord.Geometry.Min(point => point.X))
                                                             .ToList();
                            foreach (var chord in orderedChords)
                            {
                                var newChord = new Chord(chord, $"{truss.Truss.Name} ({(i + 1)})");
                                Chords.Add(newChord);
                            }
                        }
                    }
                }
                dataGridView1.DataSource = Chords;
            }

        }


        //load tres into Paragon
        // load count file
        // get the trusses back from paragon
        // create list of chords, Truss Name, Chord Name, Length, Grade, Treatment, width
        // display chords in datagridview

    }
}
