using System.Runtime.Serialization;

namespace Notification.Consumer.Exceptions;

[Serializable]
public class NotificationEventException : Exception
{
  public NotificationEventException(string message) : base(message)
  {
  }

  protected NotificationEventException(SerializationInfo info,
    StreamingContext context) : base(info, context)
  {
  }
}