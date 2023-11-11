namespace Ivet.Model
{
    public enum Multiplicity
    {
        /// <summary>
        /// Allows multiple edges of the same label between any pair of vertices. In other words,
        /// the graph is a multi graph with respect to such edge label. There is no constraint on
        /// edge multiplicity.
        /// </summary>
        MULTI,
        /// <summary>
        /// Allows at most one edge of such label between any pair of vertices. In other words,
        /// the graph is a simple graph with respect to the label. Ensures that edges are unique for
        /// a given label and pairs of vertices.
        /// </summary>
        SIMPLE,
        /// <summary>
        /// Allows at most one outgoing edge of such label on any vertex in the graph but places no
        /// constraint on incoming edges. The edge label mother is an example with MANY2ONE multiplicity
        /// since each person has at most one mother but mothers can have multiple children.
        /// </summary>
        MANY2ONE,
        /// <summary>
        /// Allows at most one incoming edge of such label on any vertex in the graph but places no
        /// constraint on outgoing edges. The edge label winnerOf is an example with ONE2MANY
        /// multiplicity since each contest is won by at most one person but a person can win multiple contests.
        /// </summary>
        ONE2MANY,
        /// <summary>
        /// Allows at most one incoming and one outgoing edge of such label on any vertex in the graph.
        /// The edge label marriedTo is an example with ONE2ONE multiplicity since a person is married
        /// to exactly one other person.
        /// </summary>
        ONE2ONE
    }
}
