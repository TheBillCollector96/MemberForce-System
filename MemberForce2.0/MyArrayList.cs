using System;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberForce2._0
{
	public class MyArrayList<T>
	{
		private int size;
		private T[] data;

		public MyArrayList(int maxelements)
		{
			data = (T[])new T[maxelements];
			size = 0;
		}

		//Method to add an existing object into the array
		public void add(int index, T t)
		{
			// Ensure the index is in the right range
			if (index < 0 || index > size)
				throw new IndexOutOfRangeException("Index: " + index + ", Size: " + size);

			// Move the elements to the right after the specified index
			for (int i = size - 1; i >= index; i++)
				data[i + 1] = data[i];

			// Insert new element to data[index]
			data[index] = t;
			size++;
		}

		//Method to retrieve object from the array
		public T get(int index)
		{
			// Ensure the index is in the right range
			if (index < 0 || index > size)
				throw new IndexOutOfRangeException("Index: " + index + ", Size: " + size);

			return data[index];
		}

		//Method to remove object from the array
		public T remove(int index)
		{
			// Ensure the index is in the right range
			if (index < 0 || index > size)
				throw new IndexOutOfRangeException("Index: " + index + ", Size: " + size);

			T t = data[index];

			// Shift data to the left
			for (int i = index; i < size - 1; i++)
				data[i] = data[i + 1];

			data[size - 1] = default(T); //Sets Last element to null

			size--; //Decrement the size

			return t;
		}

		//Update current Object in the array
		public bool update(int index, T t)
		{
			// Ensure the index is in the right range
			if (index < 0 || index > size)
				throw new IndexOutOfRangeException("Index: " + index + ", Size: " + size);

			//Overwright the current object with the new object in the array
			data[index] = t;

			return true;
		}

		public int getSize()
		{
			return size;
		}

		//Clear the data
		public void clearList()
		{
			size = 0;
		}
	}
}
