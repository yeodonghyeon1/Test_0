using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Location;
using Xamarin.Essentials;
using System;
using Android.Gms.Tasks;
using Android.Gms.Maps.Model;



namespace Test_0
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MapActivity : AppCompatActivity, IOnMapReadyCallback
    {
        TextView txtv;
        double lat, lng;

        protected override async void OnCreate(Bundle savedInstanceState) //async를 써서 oncreate를  비동기 메서드로 만든다.

        {
            base.OnCreate(savedInstanceState); //??

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource

            SetContentView(Resource.Layout.activity_main); //xml을 화면에 뿌려줌
            // 로그인



            var mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);// 아마도 Onreadymap을 호출함 


            var Lastlocation = await Geolocation.GetLastKnownLocationAsync();// 마지막 위치를 location에 저장 

            //예외처리를 위해서 try catch 문을 사용
            try
            {

                if (Lastlocation != null)
                {
                    //위도 경도값을 따로 저장
                    lat = Lastlocation.Latitude;
                    lng = Lastlocation.Longitude;

                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }


            //두위치간 거리 측정 
            Location test1 = new Location(42.358056, -71.063611);
            Location test2 = new Location(37.783333, -122.416667);
            double def_distance_k = Location.CalculateDistance(test2, test1, DistanceUnits.Kilometers);



            //버튼이벤트 연결부
            txtv = FindViewById<TextView>(Resource.Id.textView1);
            FindViewById<Button>(Resource.Id.button1).Click += (o, e) =>
            Def(def_distance_k);
        }

        //두값을 비교하여 문자열을 text란에 입력해줌
        //두값을 비교하는걸 반복문에 넣어서 전 이용자들의 거리값을 다 비교해봐야함....
        public void Def(double dst_k)
        {
            if (dst_k < 10)
            {
                txtv.Text = "10km내에 사람찾음";
            }
            else txtv.Text = "못찾음....";

        }


        //처음부터있던거임....  OnRequestPermissionsResult()
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }





        //map 설정
        public void OnMapReady(GoogleMap map)
        {
            //marker 의 옵션 설정
            MarkerOptions mo = new MarkerOptions();
            LatLng test = new LatLng(lat, lng);  //마지막위치 값을 저장하고있음


            mo.SetPosition(test);
            mo.SetTitle("test");
            map.AddMarker(mo);

            //카메라 구현부
            LatLng location = new LatLng(lat, lng);

            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(18);
            builder.Bearing(155);
            builder.Tilt(55);

            CameraPosition cameraPosition = builder.Build();

            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            ////버튼1을 눌럿을떄 위치이동하도록 클릭이벤트에 추가함
            //FindViewById<Button>(Resource.Id.button1).Click += (o, e) =>
            //map.MoveCamera(cameraUpdate);










            //구글맵 속성부분(UI)
            map.UiSettings.ZoomControlsEnabled = true;
            map.UiSettings.CompassEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = true; //추가했는데 버튼이 보이지않음, 기능구현을 해야 뜨나봄



            // 내위치 
            map.MyLocationEnabled = true;

        }

    }
}