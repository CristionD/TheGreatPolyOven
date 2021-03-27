/* 
 * Retains basic information on a pastry.
 * 
 * Author: Cristion Dominguez
 * Latest Revision: 27 Sept. 2020
 */

using System;

public struct pastryInfo : IEquatable<pastryInfo>
{
    public string name;
    public int sweetnessLevel;

    /// <summary>
    /// Compares the name and sweetness level of this pastry to some other pastry and if they are the same, return true; otherwise, return false.
    /// </summary>
    /// <param name="somePastry"> A variable of pastryInfo for comparison. </param>
    /// <returns> Boolean representing whether the other variable of pastryInfo is the same. </returns>
    public bool Equals(pastryInfo somePastry) => this.name == somePastry.name && this.sweetnessLevel == somePastry.sweetnessLevel;
}
