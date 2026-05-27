using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace DTT.Utils.Workflow
{
    ///<summary>
    /// A grid representation of a data array.
    ///</summary>
    public class GridBase<T>
    {
        /// <summary>
        /// The minimum amount of rows or columns used for the grid.
        /// </summary>
        public const int MIN_ROW_OR_COLUMN_SIZE = 2;

        /// <summary>
        /// The amount of columns used in the grid.
        /// </summary>
        public int Columns
        {
            get => _columns;
            set
            {
                if (value < MIN_ROW_OR_COLUMN_SIZE)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _columns = value;
                
                Resize();
            }
        }

        /// <summary>
        /// The amount of used in the grid.
        /// </summary>
        public int Rows
        {
            get => _rows;
            set
            {
                if (value < MIN_ROW_OR_COLUMN_SIZE)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _rows = value;
                
                Resize();
            }
        }

        /// <summary>
        /// Returns a value from the grid.
        /// </summary>
        /// <param name="x">The x coordinate of the value.</param>
        /// <param name="y">The y coordinate of the value.</param>
        public T this[int x, int y] => GetValue(x, y);

        /// <summary>
        /// Returns a value from the grid.
        /// </summary>
        /// <param name="coordinates">The coordinates of the value.</param>
        public T this[Vector2Int coordinates] => GetValue(coordinates.x, coordinates.y);
        
        /// <summary>
        /// The collection of values used by the grid.
        /// </summary>
        public ReadOnlyCollection<T> Values => Array.AsReadOnly(_values);
        
        /// <summary>
        /// The array values used by the grid.
        /// </summary>
        private T[] _values;
        
        /// <summary>
        /// The amount of columns used in the grid.
        /// </summary>
        private int _columns;

        /// <summary>
        /// The amount of rows used in the grid.
        /// </summary>
        private int _rows;

        /// <summary>
        /// Creates a new default grid with a minimum size.
        /// </summary>
        public GridBase()
        {
            _columns = MIN_ROW_OR_COLUMN_SIZE;
            _rows = MIN_ROW_OR_COLUMN_SIZE;
            
            Resize();
        }
        
        /// <summary>
        /// Creates a new grid with a given amount of columns and rows.
        /// </summary>
        /// <param name="columns">The amount of columns used in the grid.</param>
        /// <param name="rows">The amount of rows used in the grid.</param>
        public GridBase(int columns, int rows)
        {
            if (columns < MIN_ROW_OR_COLUMN_SIZE)
                throw new ArgumentOutOfRangeException(nameof(columns));

            if (rows < MIN_ROW_OR_COLUMN_SIZE)
                throw new ArgumentOutOfRangeException(nameof(rows));
            
            _rows = rows;
            _columns = columns;
            
            Resize();
        }

        /// <summary>
        /// Creates a new grid with a given amount of columns and rows.
        /// </summary>
        /// <param name="columns">The amount of columns used in the grid.</param>
        /// <param name="rows">The amount of rows used in the grid.</param>
        /// <param name="values">The values used by the grid.</param>
        public GridBase(int columns, int rows, T[] values) : this(columns, rows)
        {
            _columns = columns;
            _rows = rows;
            
            Resize();
            Populate(values);
        }

        /// <summary>
        /// Creates a new grid with a given amount of values.
        /// </summary>
        /// <param name="values">The values to initialize the grid with.</param>
        public GridBase(T[,] values)
        {
            _columns = values.GetLength(0);
            _rows = values.GetLength(1);
            
            Populate(values);
        }

        /// <summary>
        /// Populates the grid with values to use.
        /// </summary>
        /// <param name="values">The values to be used by the grid.</param>
        public void Populate(T[] values) => values.CopyTo(_values, 0);

        /// <summary>
        /// Populates the grid with values to use.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="forceResize">Whether to force a resize of the grid if the given size is bigger.</param>
        public void Populate(T[,] values, bool forceResize = true)
        {
            T[] flattened = values.Cast<T>().ToArray();
            int columns = values.GetLength(0);
            int rows = values.GetLength(1);

            if(forceResize)
                Resize(columns, rows);
            
            Populate(flattened);
        }

        /// <summary>
        /// Returns a value from the grid.
        /// </summary>
        /// <param name="coordinates">The coordinates of the value.</param>
        /// <returns>The value from the grid.</returns>
        public T GetValue(Vector2Int coordinates) => GetValue(coordinates.x, coordinates.y);
        
        /// <summary>
        /// Returns a value from the grid.
        /// </summary>
        /// <param name="x">The x coordinate of the value.</param>
        /// <param name="y">The y coordinate of the value.</param>
        /// <returns>The value from the grid.</returns>
        public T GetValue(int x, int y) => IsInvalid(x, y) ? default : _values[GetIndex(x, y)];
        
        /// <summary>
        /// Swaps values of two different grid coordinates. Uses the 'GetValue' method to
        /// swap the values which means default values will be used if coordinates
        /// are out of bounds.
        /// </summary>
        /// <param name="coordinates1">The first grid coordinate.</param>
        /// <param name="coordinates2">The second grid coordinate.</param>
        public void SwapValues(Vector2Int coordinates1, Vector2Int coordinates2) => 
            SwapValues(coordinates1.x, coordinates1.y, coordinates2.x, coordinates2.y);
    
        /// <summary>
        /// Swaps values of two different grid coordinates. Uses the 'GetValue' method to
        /// swap the values which means default values will be used if coordinates
        /// are out of bounds.
        /// </summary>
        /// <param name="x1">The x coordinate of the first value</param>
        /// <param name="y1">The y coordinate of the first value</param>
        /// <param name="x2">The x coordinate of the second value</param>
        /// <param name="y2">The y coordinate of the second value</param>
        public void SwapValues(int x1, int y1, int x2, int y2)
        {
            T temp = GetValue(x1, y1);
            
            SetValue(x1, y1, GetValue(x2, y2));
            SetValue(x2, y2, temp);
        }

        /// <summary>
        /// Sets the value at a given grid coordinate.
        /// </summary>
        /// <param name="coordinates">The coordinates at which to set the value.</param>
        /// <param name="newValue">The new value.</param>
        public void SetValue(Vector2Int coordinates, T newValue) => SetValue(coordinates.x, coordinates.y, newValue);

        /// <summary>
        /// Sets the value at a given grid coordinate.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="newValue">The new value to assign.</param>
        public void SetValue(int x, int y, T newValue)
        {
            if (IsInvalid(x, y))
                return;

            _values[GetIndex(x, y)] = newValue;
        }
        
        /// <summary>
        /// Returns the array index based on given coordinates.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>The array index.</returns>
        public int GetIndex(int x, int y) => (Columns * y) + x;
        
        /// <summary>
        /// Returns whether given coordinates lie within the grid.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>Whether given coordinates lie within the grid.</returns>
        public bool IsInvalid(int x, int y) => (x < 0 || x >= Columns) || (y < 0 || y >= Rows);

        /// <summary>
        /// Resizes the grid using given column and row values.
        /// </summary>
        /// <param name="columns">The amount of columns used in the grid.</param>
        /// <param name="rows">The amount of rows used in the grid.</param>
        public void Resize(int columns, int rows)
        {
            _columns = columns;
            _rows = rows;

            Resize();
        }
        
        /// <summary>
        /// Resizes the grid using stored column and row values.
        /// </summary>
        private void Resize()
        {
            int size = _columns * _rows;
            if (_values == null)
            {
                _values = new T[_columns * _rows];
            }
            else
            {
                Array.Resize(ref _values, size);
            }
        }
    }
}