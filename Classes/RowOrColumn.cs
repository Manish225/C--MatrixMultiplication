using System;

namespace MatrixMultiplication.Classes
{
	public class RowOrColumn
	{
		public RowOrColumn()
		{
			
		}

		public string Success;

		public List<int> Value;

        public static implicit operator RowOrColumn(HttpResponseMessage v)
        {
            throw new NotImplementedException();
        }
    }
}

