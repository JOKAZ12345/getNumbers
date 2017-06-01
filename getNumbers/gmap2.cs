using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace getNumbers
{
   
    public partial class gmap2 : Form
    {
        public gmap2()
        {
            InitializeComponent();
        }



        private void gmap2_Load(object sender, EventArgs e)
        {
            gmap.MapProvider = GoogleMapProvider.Instance;
            gmap.IgnoreMarkerOnMouseWheel = true;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gmap.SetPositionByKeywords("Figueira da Foz, Portugal");
            gmap.ShowCenter = false;
            gmap.DragButton = MouseButtons.Left;
        }

        private void addPonto(double lat, double lon, string desc, GMapOverlay markers, string url, string data)
        {
            var diff = DateTime.Now - Convert.ToDateTime(data);
            var hours = diff.Hours;

            if (diff.Days < 30)
            {
                var gmarker = new GMarkerGoogle(new PointLatLng(lat, lon), GMarkerGoogleType.blue_small)
                {
                    ToolTipText = desc,
                    ToolTipMode = MarkerTooltipMode.OnMouseOver,
                    Tag = url
                };

                markers.Markers.Add(gmarker);
            }

            else if (diff.Days > 170 && diff.Days < 200)
            {
                var gmarker = new GMarkerGoogle(new PointLatLng(lat, lon), GMarkerGoogleType.orange_small)
                {
                    ToolTipText = desc,
                    ToolTipMode = MarkerTooltipMode.OnMouseOver,
                    Tag = url
                };

                markers.Markers.Add(gmarker);
            }

            else
            {
                var color = GMarkerGoogleType.green_small;

                if (desc.ToLower().Contains("max"))
                {
                    color = GMarkerGoogleType.brown_small;
                }

                var gmarker = new GMarkerGoogle(new PointLatLng(lat, lon), color)
                {
                    ToolTipText = desc,
                    ToolTipMode = MarkerTooltipMode.OnMouseOver,
                    Tag = url
                };

                markers.Markers.Add(gmarker);
            }
        }

        private void gmap_OnMarkerClick(object sender, EventArgs eventArgs)
        {
            MessageBox.Show($@"Marker {((GMapMarker)sender).Tag} was clicked.");
            System.Diagnostics.Process.Start((string) ((GMapMarker) sender).Tag);
        }

        public void addMarkersPrabitar()
        {
            var markers = new GMapOverlay("markers");

            var db = new prabitarDataContext();

            foreach (var imovel in db.Imovels)
            {
                var x = new PointLatLng();
                var res = gmap.GetPositionByKeywords(imovel.Localizacao, out x);

                if (res == GeoCoderStatusCode.G_GEO_SUCCESS)
                {   //TODO: ADD DATA DO OUTRO ANUNCIO
                    var gmarker = new GMarkerGoogle(new PointLatLng(x.Lat, x.Lng), GMarkerGoogleType.red_pushpin)
                    {
                        ToolTipText = imovel.Tipo,
                        ToolTipMode = MarkerTooltipMode.OnMouseOver,
                        Tag = imovel.Localizacao
                    };

                    markers.Markers.Add(gmarker);
                }
            }

            gmap.Overlays.Add(markers);
        }

        public void addMarkers(List<Marker> markersList)
        {
            var markers = new GMapOverlay("markers");

            var db = new prabitarDataContext();

            foreach (var item in db.Potencials)
            {
                var x = new PointLatLng();
                var res = gmap.GetPositionByKeywords(item.TituloAnuncio, out x);

                if (res == GeoCoderStatusCode.G_GEO_SUCCESS)
                {   //TODO: ADD DATA DO OUTRO ANUNCIO
                    addPonto(x.Lat, x.Lng, item.TituloAnuncio + "\n" + item.Telefone + "\n" + item.Preco, markers, item.URL, null);
                }
            }

            foreach (var mark in markersList)
            {
                addPonto(mark.Latitude, mark.Longitude, mark.tooltiptext, markers, mark.url, mark.data);
            }

            gmap.Overlays.Add(markers);

            addMarkersPrabitar();
        }

        private void gmap_Load(object sender, EventArgs e)
        {

        }

        private void gmap_Click(object sender, EventArgs e)
        {
            /*if (((GMapControl) sender).IsMouseOverMarker)
            {
                
            }*/
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {

        }
    }

    public class Marker
    {
        public double Latitude;
        public double Longitude;
        public string tooltiptext;
        public string url;
        public string data;

        public Marker(string tooltiptext, double longitude, double latitude, string url, string data)
        {
            this.tooltiptext = tooltiptext;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.url = url;
            this.data = data;
        }
    }
}
