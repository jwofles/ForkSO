﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TSO.Content.framework;
using System.IO;
using TSO.Common.utils;
using TSOClient.Code.Utils;

namespace TSO.Content.codecs
{
    /// <summary>
    /// Codec for textures (*.jpg).
    /// </summary>
    public class TextureCodec : IContentCodec<Texture2D>
    {
        private GraphicsDevice Device;
        private bool Mask = false;
        private uint[] MaskColors = null;

        /// <summary>
        /// Creates a new instance of TextureCodec.
        /// </summary>
        /// <param name="device">A GraphicsDevice instance.</param>
        public TextureCodec(GraphicsDevice device)
        {
            this.Device = device;
        }

        /// <summary>
        /// Creates a new instance of TextureCodec.
        /// </summary>
        /// <param name="device">A GraphicsDevice instance.</param>
        /// <param name="maskColors">A list of masking colors to use for this texture.</param>
        public TextureCodec(GraphicsDevice device, uint[] maskColors)
        {
            this.Device = device;
            this.Mask = true;
            this.MaskColors = maskColors;
        }

        #region IContentCodec<Texture2D> Members

        public Texture2D Decode(System.IO.Stream stream)
        {
            /**
             * This may not be the right way to get the texture to load as ARGB but it works :S
             */
            Texture2D texture = null;
            if(Mask)
            {
                var textureParams = Texture2D.GetCreationParameters(Device, stream);
                textureParams.Format = SurfaceFormat.Color;
                stream.Seek(0, SeekOrigin.Begin);
                texture = Texture2D.FromFile(Device, stream, textureParams);

                TextureUtils.ManualTextureMaskSingleThreaded(ref texture, MaskColors);
            }
            else
            {
                texture = Texture2D.FromFile(Device, stream);
            }

            return texture;
        }

        #endregion
    }
}