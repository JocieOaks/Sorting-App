using Sorting_App.Models;

namespace Select_Sort
{
    /// <summary>
    /// The <see cref="SelectSort"/> class is a sorting algorithm designed for using user input and decision making between options.
    /// Select sort avoids fatigue from seeing the same option presented multiple times, so that both options have some level of novelty.
    /// This makes the algorithm rather inefficient, as it actively avoids the use of pivots used in algorithms like quicksort. Instead it allows for 
    /// degree based comparison to make predictions for what comparisons will provide the most novel information.
    /// </summary>
    public static class SelectSort
    {
        /// <summary>
        /// Adjusts the scores of the <see cref="SortingElement"/>s that have been modified, based on a force/potential energy adjacent model.
        /// </summary>
        /// <param name="elements">The list of <see cref="SortingElement"/>s whose scores are being adjusted.</param>
        /// <param name="comparisons">A table of the relationships between the different <see cref="SortingElement"/>, created by <see cref="GetComparisonMatrix(List{ElementComparison}, int)"/>.</param>
        /// <param name="degrees">The max degree of the comparisons, defaults to 3.</param>
        public static void AdjustScores(List<SortingElement> elements, int?[,] comparisons, int degrees = 3)
        {
            bool adjustmentsMade;
            int loopCount = 0;
            do
            {
                loopCount++;
                adjustmentsMade = false;
                for (int i = 0; i < elements.Count; i++)
                {
                    SortingElement element = elements.First(x => x.Index == i);
                    int forceCount = 0;
                    float potential = -element.Score;
                    for (int j = 0; j < elements.Count; j++)
                    {
                        float? force = -comparisons[i, j];

                        if (force.HasValue)
                        {
                            forceCount++;
                            potential += (MathF.Sign(force.Value) * 10 + elements.First(x => x.Index == j).Score - element.Score) * MathF.Abs(force.Value) / degrees;
                        }
                    }

                    if (forceCount > 0)
                    {
                        float absForce = 1 - 10 / (MathF.Abs(potential / forceCount) + 10);

                        element.Push = MathF.Sign(potential) * absForce / 10;
                        if (absForce > 0.001)
                            adjustmentsMade = true;
                    }
                }

                foreach (SortingElement element in elements)
                {
                    element.OnPush();
                }
            } while (adjustmentsMade && loopCount < 500);
        }

        /// <summary>
        /// Initializes all the <see cref="SortingElement"/>s and the <see cref="ElementComparison"/>s that are used by <see cref="Sort"/>.
        /// </summary>
        /// <param name="elements">The list of <see cref="Element"/>s being sorted.</param>
        /// <param name="sortingElements">The newly constructed list of <see cref="SortingElement"/>s.</param>
        /// <param name="elementComparisons">The newly constructed list of <see cref="ElementComparison"/>s.</param>
        public static void BuildSortDatabase(List<Element> elements, out List<SortingElement> sortingElements, out List<ElementComparison> elementComparisons)
        {
            sortingElements = new List<SortingElement>(elements.Count);
            elementComparisons = new List<ElementComparison>();

            for (int i = 0; i < elements.Count; i++)
            {
                sortingElements.Add(new SortingElement(elements[i], i));
            }

            for (int i = 0; i < elements.Count; i++)
            {
                for (int j = i + 1; j < elements.Count; j++) if(i != j)
                {
                    // To avoid any effect that might occur due to element order, option orders are semi-random.
                    // (Not random per se, but certain options won't exclusively be on one side or the other)
                    if (i + j % 2 == 0)
                        elementComparisons.Add(new ElementComparison { FirstElement = sortingElements[i], SecondElement = sortingElements[j] });
                    else
                        elementComparisons.Add(new ElementComparison { FirstElement = sortingElements[j], SecondElement = sortingElements[i] });
                }
            }
        }

        /// <summary>
        /// Groups the data points into a number of clusters, a la a tier list.
        /// </summary>
        /// <param name="clusterCount">Number of tiers to divide the data into.</param>
        /// <param name="elements">The <see cref="SortingElement"/>s being clustered.</param>
        /// <returns>Returns an array of lists containing the elements in each cluster, and the clusters average score.</returns>
        public static (List<Element>, float median)[] Cluster(int clusterCount, List<SortingElement> elements)
        {
            Random rng = new();
            float[] clusterPoint = new float[clusterCount];
            for (int i = 0; i < clusterCount; i++)
            {
                float nextPoint;
                do
                {
                    nextPoint = elements[rng.Next(elements.Count)].Score;
                } while (clusterPoint.Any(x => x == nextPoint));

                clusterPoint[i] = nextPoint;
            }

            bool changed;
            do
            {
                changed = false;
                int[] clusterMemberCount = new int[clusterCount];
                float[] clusterSum = new float[clusterCount];
                foreach (SortingElement element in elements)
                {
                    int clusterIndex = FindNearestPoint(clusterPoint, element);
                    clusterMemberCount[clusterIndex]++;
                    clusterSum[clusterIndex] += element.Score;
                }

                for (int i = 0; i < clusterCount; i++)
                {
                    float average = clusterSum[i] / clusterMemberCount[i];
                    if (average != clusterPoint[i])
                    {
                        clusterPoint[i] = average;
                        changed = true;
                    }
                }

            } while (changed);

            (List<Element> members, float)[] clusters = new (List<Element>, float)[clusterCount];
            for (int i = 0; i < clusterCount; i++)
            {
                clusters[i] = (new(), clusterPoint[i]);
            }

            foreach (SortingElement element in elements)
            {
                clusters[FindNearestPoint(clusterPoint, element)].members.Add(element.Element);
            }

            return clusters;
        }

        /// <summary>
        /// Finds the best comparison to present to the user.
        /// </summary>
        /// <param name="elementComparisons">All of the <see cref="ElementComparison"/>s, including those that can be presented to
        /// the user, and those that have already been evaluated.</param>
        /// <param name="selectionCount">The current count of how many choices the user has made.</param>
        /// <returns>Returns the best <see cref="ElementComparison"/> to present to the user. Can be null if all <see cref="ElementComparison"/>s
        /// have been solved, signifying that the list is sorted.</returns>
        public static ElementComparison? FindNextComparison(List<ElementComparison> elementComparisons, int selectionCount)
        {
            ElementComparison? next = SelectNearby(elementComparisons, selectionCount);

            next?.FirstElement.OptionPresented(selectionCount);
            next?.SecondElement.OptionPresented(selectionCount);

            return next;
        }

        /// <summary>
        /// Constructs a 2D array that can be quickly traversed, detailed by a list of <see cref="ElementComparison"/>s.
        /// </summary>
        /// <param name="elementComparisons">The <see cref="ElementComparison"/>s that the matrix is representing.</param>
        /// <param name="elementCount">The number of different <see cref="SortingElement"/>s within the list of <see cref="ElementComparison"/>s.</param>
        /// <returns>Returns the 2D matrix that corresponds to the <see cref="ElementComparison"/>s.</returns>
        public static int?[,] GetComparisonMatrix(List<ElementComparison> elementComparisons, int elementCount)
        {
            int?[,] comparisons = new int?[elementCount, elementCount];
            foreach (ElementComparison elementComparison in elementComparisons)
            {
                comparisons[elementComparison.FirstElement.Index, elementComparison.SecondElement.Index] = elementComparison.PreferenceDegree;
                comparisons[elementComparison.SecondElement.Index, elementComparison.FirstElement.Index] = -elementComparison.PreferenceDegree;
            }

            return comparisons;
        }

        /// <summary>
        /// Sorts the list of <see cref="Element"/>s, based on the <see cref="ElementComparison"/>s.
        /// </summary>
        /// <param name="elements">An array containing the <see cref="SortingElement"/>s being sorted.</param>
        /// <param name="comparisons">The <see cref="ElementComparison"/>s containing the relations between each <see cref="SortingElement"/>.</param>
        /// <returns>Returns thesorted list of <see cref="Element"/>s.</returns>
        public static IEnumerable<Element> GetOrdereredList(SortingElement[] elements, List<ElementComparison> comparisons)
        {
            Quicksort(elements, 0, elements.Length - 1, GetComparisonMatrix(comparisons, elements.Length));
            foreach (SortingElement element in elements)
            {
                yield return element.Element;
            }
        }

        /// <summary>
        /// Assigns values to the comparison matrix.
        /// </summary>
        /// <param name="first">The first <see cref="SortingElement"/> in the comparison.</param>
        /// <param name="second">The second <see cref="SortingElement"/> in the comparison.</param>
        /// <param name="degree">The degree to which <c>first</c> is ranked above <c>second</c> (negative if first is ranked lower).</param>
        /// <param name="comparisons">The list of all <see cref="ElementComparison"/>s.</param>
        /// <param name="degrees">The max degree of the comparisons, defaults to 3.</param>
        public static void SetComparison(SortingElement first, SortingElement second, int degree, List<ElementComparison> comparisons, int degrees = 3)
        {
            ElementComparison primaryComparison = comparisons.First(x => x.HasElements(first, second));
            if (primaryComparison.Solved)
                return;

            primaryComparison.SetPreference(first, degree);

            foreach (ElementComparison comparison in comparisons)
            {
                if (comparison.PreferenceDegree.HasValue)
                {
                    if (comparison.HasElement(first))
                    {
                        SortingElement third = first == comparison.FirstElement ? comparison.SecondElement : comparison.FirstElement;

                        int thirdPreference = comparison.GetPreference(third)!.Value;

                        if (Math.Sign(thirdPreference) == Math.Sign(degree))
                        {
                            SetComparison(third, second, Math.Max(Math.Min(thirdPreference + degree / 2, degrees), -degrees), comparisons, degrees);
                        }
                    }
                    else if (comparison.HasElement(second))
                    {
                        SortingElement third = second == comparison.FirstElement ? comparison.SecondElement : comparison.FirstElement;

                        int thirdPrefrence = comparison.GetPreference(third)!.Value;

                        if (Math.Sign(thirdPrefrence) == Math.Sign(-degree))
                        {
                            SetComparison(third, first, Math.Max(Math.Min(thirdPrefrence - degree / 2, degrees), -degrees), comparisons, degrees);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determine which points is nearest to the score of a given <see cref="SortingElement"/>.
        /// </summary>
        /// <param name="points">An array of points.</param>
        /// <param name="element">The <see cref="SortingElement"/> being analyzed.</param>
        /// <returns>The index of the point in <c>points</c> that is nearest to <c>element</c>.</returns>
        private static int FindNearestPoint(float[] points, SortingElement element)
        {
            float closestDistance = float.PositiveInfinity;
            int closestCluster = -1;
            for (int i = 0; i < points.Length; i++)
            {
                float distance = MathF.Abs(points[i] - element.Score);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCluster = i;
                }

            }

            return closestCluster;
        }

        /// <summary>
        /// Sorts the elements in the list once the full relations between each element has been mapped.
        /// </summary>
        /// <param name="list">The array of all <see cref="SortingElement"/>s.</param>
        /// <param name="low">The index of the lowest member of the list currently being sorted.</param>
        /// <param name="high">The index of the highest memeber of the list currently being sorted.</param>
        /// <param name="comparisons">A 2D array detailing the relationships between every <see cref="SortingElement"/> in <c>list</c>.</param>
        private static void Quicksort(SortingElement[] list, int low, int high, int?[,] comparisons)
        {
            if (high < low)
                return;
            int pivotIndex = list[high].Index;
            for (int i = low; i <= high; i++)
            {
                SortingElement element = list[i];
                if (comparisons[pivotIndex, element.Index] < 0)
                {
                    (list[low], list[i]) = (element, list[low]);
                    low++;
                }
                else if (comparisons[pivotIndex, element.Index] == 0)
                    throw new InvalidOperationException("Comparison matrix is incomplete. Cannot continue sorting.");
            }
            (list[low], list[high]) = (list[high], list[low]);

            Quicksort(list, 0, low - 1, comparisons);
            Quicksort(list, low + 1, high, comparisons);
        }

        /// <summary>
        /// Chooses the next comparison to check based on how close the two options are expected to be.
        /// </summary>
        /// <param name="comparisons">The list of all possible <see cref="ElementComparison"/>s, including those that have already been solved.</param>
        /// <param name="SelectionCount">The current count of how many choices the user has made.</param>
        /// <returns>Returns the next comparison to make.</returns>
        private static ElementComparison? SelectNearby(List<ElementComparison> comparisons, int SelectionCount)
        {
            float bestValue = float.PositiveInfinity;
            ElementComparison? best = default;
            foreach (ElementComparison comparison in comparisons) if(!comparison.Solved)
            {
                SortingElement first = comparison.FirstElement;
                SortingElement second = comparison.SecondElement;

                float value = MathF.Abs(first.Score - second.Score);
                value += first.Fatigue(SelectionCount);
                value += second.Fatigue(SelectionCount);

                if (value < bestValue)
                {
                    bestValue = value;
                    best = comparison;
                }

            }
            return best;
        }
    }
}
