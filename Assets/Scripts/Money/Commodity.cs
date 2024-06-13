namespace Money
{
    public interface Commodity
    {
        int NeedCost();

        bool EnableBuy();

        void Buy();
    }
}