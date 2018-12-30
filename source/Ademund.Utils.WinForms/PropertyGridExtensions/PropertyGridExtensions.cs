using System.Reflection;
using System.Windows.Forms;

namespace Ademund.Utils.PropertyGridExtensions
{
    public static class PropertyGridExtensions
	{
		/// <summary>
		/// Gets the (private) PropertyGridView instance.
		/// </summary>
		/// <param name="propertyGrid">The property grid.</param>
		/// <returns>The PropertyGridView instance.</returns>
		public static object GetPropertyGridView(this PropertyGrid propertyGrid)
		{
			//private PropertyGridView GetPropertyGridView();
			//PropertyGridView is an internal class...
			MethodInfo methodInfo = typeof(PropertyGrid).GetMethod("GetPropertyGridView", BindingFlags.NonPublic | BindingFlags.Instance);
			return methodInfo.Invoke(propertyGrid, new object[] { });
		}

		public static VScrollBar GetVScrollBar(this PropertyGrid propertyGrid)
		{
            object propertyGridView = propertyGrid.GetPropertyGridView();
			FieldInfo fi = propertyGridView.GetType().GetField("scrollBar", BindingFlags.NonPublic | BindingFlags.Instance);
			return (VScrollBar)fi.GetValue(propertyGridView);
		}

		public static void ScrollTo(this PropertyGrid propertyGrid, int newValue)
		{
			var scrollbar = propertyGrid.GetVScrollBar();
			scrollbar.Value = newValue;
		}

		public static void SetVScrollVisible(this PropertyGrid propertyGrid, bool visible)
		{
			var scrollbar = propertyGrid.GetVScrollBar();
			scrollbar.Visible = visible;
		}

		public static int GetVScrollMaximum(this PropertyGrid propertyGrid)
		{
			var scrollbar = propertyGrid.GetVScrollBar();
			return scrollbar.Maximum;
		}

		public static int GetVScrollMinimum(this PropertyGrid propertyGrid)
		{
			var scrollbar = propertyGrid.GetVScrollBar();
			return scrollbar.Minimum;
		}
	}
}