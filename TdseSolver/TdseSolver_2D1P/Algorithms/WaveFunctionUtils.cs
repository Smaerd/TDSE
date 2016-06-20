﻿using System;
using System.Drawing;
using ComplexS = TdseSolver.Complex;


namespace TdseSolver_2D1P
{
    /// <summary>
    /// This class provides miscellaneous utility methods for working with wavefunctions.
    /// </summary>
    class WaveFunctionUtils
    {
        /// <summary>
        /// Creates a Gaussian wavepacket with given properties.
        /// </summary>
        public static WaveFunction CreateGaussianWavePacket(
            int gridSizeX, int gridSizeY, float latticeSpacing, float mass,
            PointF packetCenter, PointF packetWidth, PointF avgMomentum)
        {
            WaveFunction wf = new WaveFunction(gridSizeX, gridSizeY, latticeSpacing);

            for (int y=0; y<gridSizeY; y++)
            {
                for (int x=0; x<gridSizeX; x++)
                {
                    ComplexS wfVal = FreeGaussianWavePacketValue(x*latticeSpacing, y*latticeSpacing, 0, packetCenter, packetWidth, avgMomentum, mass);
                    wf.RealPart[x][y] = wfVal.Re;
                    wf.ImagPart[x][y] = wfVal.Im;
                }
            }

            wf.Normalize();
            return wf;
        }


        /// <summary>
        /// Calculates the value of a (freely evolving) Gaussian wavepacket at a given location and time. 
        /// </summary>
        public static ComplexS FreeGaussianWavePacketValue(float x, float y, float t, 
            PointF initialCenter, PointF initialWidth, PointF avgMomentum, float mass )
        {
            ComplexS I = ComplexS.I;

            ComplexS effSigmaXSq = initialWidth.X*initialWidth.X + I*(t/mass);
            ComplexS effSigmaYSq = initialWidth.Y*initialWidth.Y + I*(t/mass);

            float xRel = x - initialCenter.X - avgMomentum.X*t/mass;
            float yRel = y - initialCenter.Y - avgMomentum.Y*t/mass;

            float avgMomentumSq = avgMomentum.X*avgMomentum.X + avgMomentum.Y*avgMomentum.Y;
            ComplexS expArg = I*(x*avgMomentum.X + y*avgMomentum.Y) - I*t*avgMomentumSq/(2*mass) - (xRel*xRel)/(2*effSigmaXSq) - (yRel*yRel)/(2*effSigmaYSq); 

            float rootPi = (float) Math.Sqrt( Math.PI );
            ComplexS normX = ComplexS.Sqrt( initialWidth.X/(rootPi*effSigmaXSq) );
            ComplexS normY = ComplexS.Sqrt( initialWidth.Y/(rootPi*effSigmaYSq) );

            ComplexS wfVal = normX*normY * ComplexS.Exp(expArg);

            return wfVal;
        }

    }
}