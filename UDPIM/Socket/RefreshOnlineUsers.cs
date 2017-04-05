using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Model;

namespace UDPIM.Socket
{
    public class RefreshOnlineUsers
    {
        //构造单例
        private static RefreshOnlineUsers instance = null;
        //线程睡眠时间
        private static int REFRESH_ONLINE_USERS_SLEEP_TIME = 1000 * 10; //单位ms
        private static int HEART_BEAT_TIME = 10 * 1; //单位s
        private Thread refreshThread = null;
        private RefreshOnlineUsers()
        {
            refreshThread = new Thread(refresh);
        }

        public static RefreshOnlineUsers getInstance()
        {
            if(instance==null)
            {
                instance = new RefreshOnlineUsers();
            }
            return instance;
        }

        public void refresh()
        {
            while(true)
            {
                DateTime now=System.DateTime.Now;
                //遍历在线用户，排除长时间没有发送心跳包的用户
                foreach (KeyValuePair<string, MyIPEndPoint> keyValuePair in OnlineUsers.getInstance().users)
                {
                    DateTime createTime= keyValuePair.Value.createTime;
                    if( now.Second-createTime.Second>HEART_BEAT_TIME)
                    {
                        MyIPEndPoint ipendPoint = new MyIPEndPoint();
                        //删除
                        OnlineUsers.getInstance().users.TryRemove(keyValuePair.Key,out ipendPoint);
                    }
                }

                Thread.Sleep(REFRESH_ONLINE_USERS_SLEEP_TIME);
            }
        }

        public void start()
        {
            if(refreshThread.ThreadState==ThreadState.Unstarted)
            {
                refreshThread.Start();
            }
        }

        public void stop()
        {
            if(refreshThread.ThreadState==ThreadState.Running)
            {
                refreshThread.Abort();
            }
        }
    }
}
