namespace City.Common.Signals
{
    
    public abstract class StaticSignal<TSignal>: ISignal
        where TSignal: StaticSignal<TSignal>, new()
    {
        public static readonly TSignal Instance = new TSignal();
    }
    
    public abstract class StaticValueSignal<TSignal, TValue>:  ISignal<TValue>
        where TSignal: StaticValueSignal<TSignal, TValue>, new()
    {
        public static readonly TSignal Instance = new TSignal();
        
        public TValue Value { get; set; }
        
        public static TSignal WithValue(TValue value)
        {
            Instance.Value = value;
            return Instance;
        }
    }
}