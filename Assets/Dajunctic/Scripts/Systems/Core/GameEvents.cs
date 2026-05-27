namespace Dajunctic
{
    
    public struct RequestRerollEvent : IEvent { }
    public struct RequestBuyXPEvent : IEvent { }
    public struct RequestBuyHeroEvent : IEvent { public int SlotIndex; }
    public struct RequestAddGoldEvent : IEvent { public int Amount; }
    public struct RequestToggleShopLockEvent : IEvent { }
}
