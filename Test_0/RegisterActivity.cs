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
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Text;


namespace Test_0
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]

    public class RegisterActivity : AppCompatActivity
    {
        EditText userName;
        EditText password;
        Button registerButton;
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public RegisterActivity()
        {

        }
        public RegisterActivity(Socket so)
        {
            socket = so;


        }


        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_register);

            userName = FindViewById<EditText>(Resource.Id.editText1);
            password = FindViewById<EditText>(Resource.Id.editText2);
            registerButton = FindViewById<Button>(Resource.Id.button1);

            registerButton.Click += RegisterClicked;


        }

        public void DataBroadCast(byte[] mode, byte[] data)
        {
            byte[] temp = new byte[mode.Length + data.Length];
            Array.Copy(mode, 0, temp, 0, mode.Length);
            Array.Copy(data, 0, temp, mode.Length, data.Length);
            socket.Send(temp);
        }


        // 회원가입
        private void RegisterClicked(object sender, EventArgs e)
        {

            Toast.MakeText(this, "작동!", ToastLength.Short).Show();
            // 클라이언트에서 보낼값: DataBroadCast
            string idpw = userName.Text + " " + password.Text;
            DataBroadCast(Encoding.Unicode.GetBytes("ANA"), Encoding.Unicode.GetBytes(idpw));

        }
    }
}