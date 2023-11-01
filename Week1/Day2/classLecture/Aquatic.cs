class Aquatic : Animal

{
    public bool hasFur = false;

    public int numFins;


    public Aquatic(string species, double weight, string skin, bool behaviour, int numF) : base(species, weight, skin, behaviour)
    {
        numFins = numF;

    }

    // We can override a Method

    public void makeNoise()
    {
        Console.WriteLine("Fish don't make a sound ...");
    }

}