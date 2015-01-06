using System;
using System.Collections.Generic;
using System.Text;
using ParatureAPI.PagedData;

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
            public void Go(CustomersList customerList)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                CustomersList objectlist = XmlToObjectParser.CustomerParser.CustomersFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                customerList.Customers.AddRange(objectlist.Customers);
                customerList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="chatList"></param>
            public void Go(ChatList chatList, Boolean IncludeTranscripts)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                ChatList objectlist = XmlToObjectParser.ChatParser.ChatsFillList(ar.xmlReceived, true, IncludeTranscripts, _requestdepth, _paracredentials);
                chatList.chats.AddRange(objectlist.chats);
                chatList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(AccountsList accountList)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                AccountsList objectlist = XmlToObjectParser.AccountParser.AccountsFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                accountList.Accounts.AddRange(objectlist.Accounts);
                accountList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(TicketsList ticketList)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                TicketsList objectlist = XmlToObjectParser.TicketParser.TicketsFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                ticketList.Tickets.AddRange(objectlist.Tickets);
                ticketList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(ProductsList productList)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                ProductsList objectlist = XmlToObjectParser.ProductParser.ProductsFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                productList.Products.AddRange(objectlist.Products);
                productList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(AssetsList assetList)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                AssetsList objectlist = XmlToObjectParser.AssetParser.AssetsFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                assetList.Assets.AddRange(objectlist.Assets);
                assetList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(DownloadsList downloadList)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                DownloadsList objectlist = XmlToObjectParser.DownloadParser.DownloadsFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                downloadList.Downloads.AddRange(objectlist.Downloads);
                downloadList.ApiCallResponse = ar;
                sem.Release();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="accountList"></param>
            public void Go(ArticlesList articleList)
            {
                ParaObjects.ApiCallResponse ar = ApiCallFactory.ObjectGetList(_paracredentials, _module, _Arguments);
                ArticlesList objectlist = XmlToObjectParser.ArticleParser.ArticlesFillList(ar.xmlReceived, true, _requestdepth, _paracredentials);
                articleList.Articles.AddRange(objectlist.Articles);
                articleList.ApiCallResponse = ar;
                sem.Release();
            }
        }
    }
}