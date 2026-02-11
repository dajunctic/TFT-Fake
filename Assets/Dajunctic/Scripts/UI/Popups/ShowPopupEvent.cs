using System;

namespace Dajunctic
{
    public enum PopupShowMode
    {
        DoNothing,
        DismissCurrent,
        PauseCurrent,
        DismissAll
    }

    public struct ShowPopupEvent : IEvent
    {
        public Type PopupType;
        public PopupShowMode ShowMode;
        public object Data;

        public ShowPopupEvent(Type popupType, PopupShowMode showMode = PopupShowMode.DoNothing, object data = null)
        {
            PopupType = popupType;
            ShowMode = showMode;
            Data = data;
        }
    }
}
