using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace BulletTitle
{
    public class BulletItem
    {
        public string bullet_text;
        public Int64 bullet_time;
        public int bullet_pos; //0 for top, 1 for medium, 2 for bottom;
        public BulletItem(string text, Int64 time, int pos)
        {
            this.bullet_text = text;
            this.bullet_time = time;
            this.bullet_pos = pos;
        }
        public static void runBullet(object obj){
            Type objType = obj.GetType();
            switch (objType.Name)
            {
                case "TextBlock":
                    TextBlock newobj = (TextBlock)obj;
                    ThicknessAnimation thickAnimation = new ThicknessAnimation();
                    thickAnimation.From = newobj.Margin;
                    thickAnimation.To = new Thickness(-1*(System.Windows.SystemParameters.PrimaryScreenWidth), newobj.Margin.Top, newobj.Margin.Right, newobj.Margin.Bottom);
                    //thickAnimation.To = new Thickness(0, newobj.Margin.Top, newobj.Margin.Right, newobj.Margin.Bottom);
                    double corrFactor = 20.0 / (newobj.Text.Length);
                    corrFactor = corrFactor > 1 ? 1 : corrFactor;
                    thickAnimation.Duration = new Duration(TimeSpan.FromSeconds(15 * corrFactor));
                    newobj.BeginAnimation(TextBlock.MarginProperty, thickAnimation);
                    break;
                default:
                    break;
            }
        }

        public static System.Windows.Media.SolidColorBrush getColor(int c)
        {
            c = c % 5;
            switch (c){
                case 0:
                    return System.Windows.Media.Brushes.Red;
                case 1:
                    return System.Windows.Media.Brushes.Yellow;
                case 2:
                    return System.Windows.Media.Brushes.White;
                case 3:
                    return System.Windows.Media.Brushes.Green;
                case 4:
                    return System.Windows.Media.Brushes.Blue;
                default:
                    return System.Windows.Media.Brushes.White;
            }
        }
    }
}
