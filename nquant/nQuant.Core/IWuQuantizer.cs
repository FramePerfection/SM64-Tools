using System.Drawing;

namespace nQuant
{
    public interface IWuQuantizer
    {
        Image QuantizeImage(Bitmap image, int maxColors, int alphaThreshold, int alphaFader);
    }
}