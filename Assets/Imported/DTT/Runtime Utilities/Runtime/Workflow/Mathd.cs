namespace DTT.Utils.Workflow
{
    /// <summary>
    /// Provides mathematical methods for double values.
    /// </summary>
    public static class Mathd
    {
        /// <summary>
        /// Returns the linearly interpolated value between double value a and b.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="target">The target value.</param>
        /// <param name="perc">The percentage value between 0 and 1.</param>
        /// <returns>The linearly interpolated value.</returns>
        public static double Lerp(double start, double target, float perc) => start + (target - start) * perc;
    }
}
