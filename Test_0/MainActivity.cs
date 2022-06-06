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
    public class MainActivity : AppCompatActivity
    {
        EditText userName;
        EditText password;
        Button loginButton;

        public static IPEndPoint ip = new IPEndPoint(IPAddress.Parse("115.40.241.164"), port: 9000);
        public static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        datamove dm = new datamove();
        RegisterActivity ra = new RegisterActivity(socket);
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_login);

            userName = FindViewById<EditText>(Resource.Id.editUsername);
            password = FindViewById<EditText>(Resource.Id.editPassword);
            loginButton = FindViewById<Button>(Resource.Id.btnLogin);

            loginButton.Click += LoginClicked;

            // 회원가입 버튼 클릭하면 회원가입 화면으로 이동
            FindViewById<Button>(Resource.Id.btnRegister).Click += (o, e) =>
            SetContentView(Resource.Layout.activity_register);
        }


        private void Receiver()
        {
            byte[] buffer = new byte[1024];
            String data;

            while (true)
            {
                socket.Receive(buffer);
                data = Encoding.Unicode.GetString(buffer);
                string mode = data.Substring(0, 3);
                string realdata = data.Substring(3);

                // 클라이언트가 받을 값
                if (mode == "LOG")  // LOG: 로그인 버튼 눌렀을 때 사용자이름, 비빌번호 전달 
                {
                   
                }
                else if(mode == "ACL")
                {
                    dm.SetSuccesLogin(mode);
                }
                else if(mode == "FAL")
                {
                    dm.SetSuccesLogin(mode);
                }

                Array.Clear(buffer, 0, buffer.Length);  // 버퍼 초기화시키는
            }
        }


        public void DataBroadCast(byte[] mode, byte[] data)
        {
            byte[] temp = new byte[mode.Length + data.Length];
            Array.Copy(mode, 0, temp, 0, mode.Length);
            Array.Copy(data, 0, temp, mode.Length , data.Length);
            socket.Send(temp);
        }


        // 로그인
        private void LoginClicked(object sender, EventArgs e)   // 로그인 버튼 클릭하면
        {

            try
            {
                socket.Connect(ip);
                new Thread(Receiver).Start();
            }
            catch (Exception ex)
            {

            }
            // 클라이언트에서 보낼값: DataBroadCast
            string idpw = userName.Text + " " + password.Text;
            DataBroadCast(Encoding.Unicode.GetBytes("LOG"), Encoding.Unicode.GetBytes(idpw));


            // 여기에서 로그인 성공 실패 판단하고싶은데,
            if ("ACL" == dm.GetSuccesLogin())  // 로그인 성공하면
            {
                // 로그인 성공 메시지
                Toast.MakeText(this, "Login successfully done!", ToastLength.Short).Show();

                // 성공 -> 맵 화면으로 전환
                SetContentView(Resource.Layout.activity_main);
            }
            else if("FAL" == dm.GetSuccesLogin())   // 로그인 실패하면
            {
                // 로그인 실패  메시지
                Toast.MakeText(this, "Wrong credentials found!", ToastLength.Long).Show();
            }
        }

        // 여기에서 로그인 성공 실패 판단해야하나요?






        // http://son10001.blogspot.com/2017/03/xamarin-android.html
        // 뒤로가기 버튼으로 종료 막기
        //public override void OnBackPressed()
        //{
        //    Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);

        //    builder.SetPositiveButton("확인", (senderAlert, args) => {
        //        Finish();
        //    });

        //    builder.SetNegativeButton("취소", (senderAlert, args) => {
        //        return;
        //    });

        //    Android.App.AlertDialog alterDialog = builder.Create();
        //    alterDialog.SetTitle("알림");
        //    alterDialog.SetMessage("프로그램을 종료 하시겠습니까?");
        //    alterDialog.Show();
        //}
    }


    class datamove
    {
        string judge= "";
        public datamove()
        {

        }

        public void SetSuccesLogin(string mode)
        {
            if(mode == "ACL")
            {
                judge = "ACL";
            }
            else if(mode == "FAL")
            {
                judge = "FAL";
            }
        }

        public string GetSuccesLogin()
        {
            return judge;
        }
    
    
    }

}