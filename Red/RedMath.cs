using UnityEngine;

namespace Red.Tools
{
    public static partial class RedTool
    {

        /// <summary>
        /// 在给定的输入范围内对值进行映射转换
        /// </summary>
        /// <param name="x">要映射的值</param>
        /// <param name="inMin">输入范围的最小值</param>
        /// <param name="inMax">输入范围的最大值</param>
        /// <param name="outMin">输出范围的最小值</param>
        /// <param name="outMax">输出范围的最大值</param>
        /// <returns>映射后的值</returns>
        public static float MapValue(float x, float inMin, float inMax, float outMin, float outMax)
        {
            // 将输入值x减去输入范围的最小值，得到一个偏移量
            float offset = x - inMin;

            // 将偏移量乘以输出范围的大小（outMax - outMin）
            float scaled = offset * (outMax - outMin);

            // 将结果除以输入范围的大小（inMax - inMin）
            float result = scaled / (inMax - inMin);

            // 将上述结果加上输出范围的最小值outMin，得到映射后的值
            return result + outMin;

            //return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }

        /// <summary>
        /// 在给定的输入范围内对值进行映射转换
        /// </summary>
        /// <param name="x">要映射的值</param>
        /// <param name="inMin">输入范围的最小值</param>
        /// <param name="inMax">输入范围的最大值</param>
        /// <param name="outMin">输出范围的最小值</param>
        /// <param name="outMax">输出范围的最大值</param>
        /// <returns>映射后的值</returns>
        public static double MapValue(double x, double inMin, double inMax, double outMin, double outMax)
        {
            double offset = x - inMin;

            double scaled = offset * (outMax - outMin);

            double result = scaled / (inMax - inMin);

            return result + outMin;
        }

        /// <summary>
        /// 超简单的插值。根据给定的参数逐渐调整当前值，使其接近目标值，并在接近目标时保持在容差范围内。
        /// </summary>
        /// <param name="currentValue">当前值</param>
        /// <param name="targetValue">目标值</param>
        /// <param name="adjustSpeed">调整速度</param>
        /// <param name="tolerance">容差范围</param>
        public static void SmoothToTarget(ref float currentValue, float targetValue, float adjustSpeed, float tolerance = 0.05f)
        {
            if (currentValue == targetValue)
            {
                return;
            }

            // 如果当前值小于目标值，逐渐增加当前值
            if (currentValue < targetValue)
            {
                currentValue += adjustSpeed * Time.deltaTime;
            }
            // 如果当前值大于目标值，逐渐减小当前值
            else if (currentValue > targetValue)
            {
                currentValue -= adjustSpeed * Time.deltaTime;
            }

            // 如果当前值与目标值的差的绝对值小于容差范围，则将当前值设为目标值
            if (Mathf.Abs(currentValue - targetValue) < tolerance)
            {
                currentValue = targetValue;
            }
        }

        /// <summary>
        /// 超简单的插值。根据给定的参数逐渐调整当前值，使其接近目标值，并在接近目标时保持在容差范围内。
        /// </summary>
        /// <param name="currentValue">当前值</param>
        /// <param name="targetValue">目标值</param>
        /// <param name="adjustSpeed">调整速度</param>
        /// <param name="tolerance">容差范围</param>
        /// <returns>调整后的值</returns>
        public static float SmoothToTarget(float currentValue, float targetValue, float adjustSpeed, float tolerance = 0.05f)
        {
            float result = currentValue;

            if (result == targetValue)
            {
                return result;
            }

            // 如果当前值小于目标值，逐渐增加当前值
            if (result < targetValue)
            {
                result += adjustSpeed * Time.deltaTime;
            }
            // 如果当前值大于目标值，逐渐减小当前值
            else if (currentValue > targetValue)
            {
                result -= adjustSpeed * Time.deltaTime;
            }

            // 如果当前值与目标值的差的绝对值小于容差范围，则将当前值设为目标值
            if (Mathf.Abs(result - targetValue) < tolerance)
            {
                result = targetValue;
            }

            return result;
        }
        /// <summary>
        /// 将给定的角度值限制在0到360度之间
        /// </summary>
        /// <param name="angle">原始角度</param>
        /// <returns>转换后的角度</returns>
        public static float ClampAngle0To360(float angle)
        {
            return (angle % 360.0f + 360.0f) % 360.0f;
        }
    }
}
