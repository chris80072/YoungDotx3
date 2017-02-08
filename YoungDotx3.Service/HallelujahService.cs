using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using YoungDotx3.DAO.MySQL;
using YoungDotx3.Domain.Hallelujah;

namespace YoungDotx3.Service
{
    public class HallelujahService
    {
        private static ILog log = LogManager.GetLogger(typeof(HallelujahService));

        public void CreateMessage(Message message)
        {
            HallelujahDAO dao = new HallelujahDAO();
            try
            {
                dao.Open(DBConn.Conn);
                message.SetIpAddress(NetWorkService.GetIpAddress());
                dao.Insert(message);
            }
            catch (Exception e)
            {
                log.Error(e, e);
            }
            finally
            {
                if (dao.Connection != null)
                    dao.Connection.Close();
            }
        }

        public Message GetMessage(string id)
        {
            HallelujahDAO dao = new HallelujahDAO();
            try
            {
                dao.Open(DBConn.Conn);
                return dao.Selete(id);
            }
            catch (Exception e)
            {
                log.Error(e, e);
                return null;
            }
            finally
            {
                if (dao.Connection != null)
                    dao.Connection.Close();
            }
        }

        public List<Message> GetMessages(string id)
        {
            HallelujahDAO dao = new HallelujahDAO();
            try
            {
                dao.Open(DBConn.Conn);
                return dao.SeleteByPage(id);
            }
            catch (Exception e)
            {
                log.Error(e, e);
                return new List<Message>();
            }
            finally
            {
                if (dao.Connection != null)
                    dao.Connection.Close();
            }
        }
    }
}
