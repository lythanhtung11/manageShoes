using System;
using System.Collections.Generic;
using System.Text;

namespace EmailServices
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
