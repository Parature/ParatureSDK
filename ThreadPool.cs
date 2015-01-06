using System;
using System.Collections.Generic;
using System.Text;

namespace ParatureAPI
{
    /// <summary>
    /// Main Thread controller class
    /// </summary>
    public class ThreadPool
    {
        /// <summary>
        /// Main Thread object, limit of 5 parallel threads
        /// </summary>
        public partial class ObjectList
        {
            private static System.Threading.Semaphore sem = new System.Threading.Semaphore(5, 5);
            private ParaCredentials _paracredentials = null;
            private ParaEnums.ParatureModule _module;
            private System.Collections.ArrayList _Arguments = null;
            private int _requestdepth;
            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="paracredentials"></param>
            /// <param name="module"></param>
            /// <param name="Arguments"></param>
            /// <param name="requestdepth"></param>
            public ObjectList(ParaCredentials paracredentials, ParaEnums.ParatureModule module, System.Collections.ArrayList Arguments, int requestdepth)
            {
                sem.WaitOne();
                _paracredentials = paracredentials;
                _module = module;
                _Arguments = new System.Collections.ArrayList(Arguments);
                _requestdepth = requestdepth;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="customerList"></param>
            public void Go(ParaObjects.CustomersList customerList)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                ParaObjects.CustomersList objectlist = XmlToObjectParser.CustomerParser.CustomersFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                customerList.Customers.AddRange(objectlist.Customers);
                customerList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="chatList"></param>
            public void Go(ParaObjects.ChatList chatList, Boolean IncludeTranscripts)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                ParaObjects.ChatList objectlist = XmlToObjectParser.ChatParser.ChatsFillList(ar.xmlReceived, true, IncludeTranscripts, _requestdepth, _paracredentials);
                chatList.chats.AddRange(objectlist.chats);
                chatList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(ParaObjects.AccountsList accountList)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                ParaObjects.AccountsList objectlist = XmlToObjectParser.AccountParser.AccountsFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                accountList.Accounts.AddRange(objectlist.Accounts);
                accountList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(ParaObjects.TicketsList ticketList)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                ParaObjects.TicketsList objectlist = XmlToObjectParser.TicketParser.TicketsFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                ticketList.Tickets.AddRange(objectlist.Tickets);
                ticketList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(ParaObjects.ProductsList productList)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                ParaObjects.ProductsList objectlist = XmlToObjectParser.ProductParser.ProductsFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                productList.Products.AddRange(objectlist.Products);
                productList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(ParaObjects.AssetsList assetList)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                ParaObjects.AssetsList objectlist = XmlToObjectParser.AssetParser.AssetsFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                assetList.Assets.AddRange(objectlist.Assets);
                assetList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(ParaObjects.DownloadsList downloadList)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                ParaObjects.DownloadsList objectlist = XmlToObjectParser.DownloadParser.DownloadsFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                downloadList.Downloads.AddRange(objectlist.Downloads);
                downloadList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(ParaObjects.ArticlesList articleList)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                ParaObjects.ArticlesList objectlist = XmlToObjectParser.ArticleParser.ArticlesFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                articleList.Articles.AddRange(objectlist.Articles);
                articleList.ApiCallResponse = ar;
                sem.Release();
            }
        }
    }
}