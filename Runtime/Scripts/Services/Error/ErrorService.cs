using System;

namespace GRAMOFON
{
    public static class ErrorService
    {
        /// <summary>
        /// This function helper for dispatch error event.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="throwError"></param>
        public static void DispatchError(string message, bool throwError = false)
        {
            ErrorEvent errorEvent = new ErrorEvent()
            {
                Message = message
            };
            
            EventService.DispatchEvent(errorEvent);

            if (throwError)
                throw new Exception(message);
        }
    }
}
