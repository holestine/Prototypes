﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace Lorenz
{
   class Pipeline : UtilMPipeline
   {
      protected int nframes;
      protected bool device_lost;

      private float m_XStart;
      private float m_YStart;
      
      public Pipeline() : base()
      {
         EnableGesture();
         nframes = 0;
         device_lost = false;
      }
      public override void OnGesture(ref PXCMGesture.Gesture data)
      {
         if (data.active)
         {
            //Write("OnGesture(" + data.label + ")");
            Console.WriteLine("OnGesture(" + data.label + ")");
         }
            

         MouseUtilities.RightClick(new Point(0,0));
      }
      public override bool OnDisconnect()
      {
         if (!device_lost) Console.WriteLine("Device disconnected");
         device_lost = true;
         return base.OnDisconnect();
      }
      public override void OnReconnect()
      {
         Console.WriteLine("Device reconnected");
         device_lost = false;
      }

      public override bool OnNewFrame()
      {
         PXCMGesture gesture = QueryGesture();
         PXCMGesture.GeoNode ndata;
         pxcmStatus sts = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out ndata);
         if (sts >= pxcmStatus.PXCM_STATUS_NO_ERROR)
         {
            MouseUtilities.SetPosition((int) ndata.positionImage.x, (int) ndata.positionImage.y);
            Console.WriteLine("node HAND_MIDDLE ({0},{1})", ndata.positionImage.x, ndata.positionImage.y);
         }

         return (++nframes < 50000);
      }

      public void Start()
      {
         if (!LoopFrames())
            Console.WriteLine("Failed to initialize or stream data");
         Dispose();
      }

      public void SetInitialPos(Point pos)
      {
         m_XStart = (int)pos.X;
         m_YStart = (int)pos.Y;
      }
   }
}
