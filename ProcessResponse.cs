using System;

namespace ReminderToDo
{
    internal class ProcessResponse
    {
        private bool _IsSuccess;
        private Int64 _Identity;
        private string _Message;
        private string _FromDate;
        private string _ToDate;
        private string _SortBy;
        private string _SortOrder;
        private string _CustomMessage;
        private bool _IsDebugMode = false;
        private bool _IsDebugStop = false;
        private string _Command;


        public string FromDate
        {
            get { return _FromDate; }
            set { _FromDate = value; }
        }

        public string ToDate
        {
            get { return _ToDate; }
            set { _ToDate = value; }
        }

        public string SortBy
        {
            get { return _SortBy; }
            set { _SortBy = value; }
        }

        public string SortOrder
        {
            get { return _SortOrder; }
            set { _SortOrder = value; }
        }

        public string Command
        {
            get { return _Command; }
            set { _Command = value; }
        }

        public bool IsDebugMode
        {
            get { return _IsDebugMode; }
            set { _IsDebugMode = value; }
        }

        public bool IsDebugStop
        {
            get { return _IsDebugStop; }
            set { _IsDebugStop = value; }
        }

        public bool IsSuccess
        {
            get { return _IsSuccess; }
            set { _IsSuccess = value; }
        }

        public Int64 Identity
        {
            get { return _Identity; }
            set { _Identity = value; }
        }

        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public string CustomMessage
        {
            get { return _CustomMessage; }
            set { _CustomMessage = value; }
        }




        public ProcessResponse()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}