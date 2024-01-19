using FormsPixelEngine.FPE.Rendering.Sprites;
using FormsPixelEngine.FPE.Rendering.Text;
using FormsPixelEngine.FPE.Sound;

namespace FormsPixelEngine.FPE.Asset
{
    /*! \class AssetManager
    \brief Class for handling game assets

    This class allows the loading and chasing of game assets   
    */
    public class AssetManager
    {
        public static string AssetPath = "./res/"; //!< the current asset path used to load assets 

        private static Dictionary<string, IAsset> _assetBase = new Dictionary<string, IAsset>();

        //! Loads and returns a given sprite 
        /*!
          \param Path The path pointing to the target image file
          \sa TextureSprite.cs
        */
        public static TextureSprite LoadTextureSprite(string _path) {
            if (_assetBase.ContainsKey(_path)) {
                if (_assetBase[_path] is TextureSprite) {
                    return _assetBase[_path] as TextureSprite;
                }
            }

            TextureSprite _sprite = new TextureSprite(AssetPath + _path);
            _assetBase.Add(_path, _sprite);
            return _sprite;
        }

        //! Loads and returns a given font 
        /*!
          \param Path The path pointing to the target FPE font file
          \sa FPEFont.cs
        */
        public static FPEFont LoadFont(string _fontpath, string _texturepath) {
            if (_assetBase.ContainsKey(_fontpath)) {
                if (_assetBase[_fontpath] is FPEFont) {
                    return _assetBase[_fontpath] as FPEFont;
                }
            }

            FPEFont _font = new FPEFont(AssetPath + _fontpath, AssetPath + _texturepath);
            _assetBase.Add(_fontpath, _font);
            return _font;
        }

        //! Loads and returns a given audio clip 
        /*!
          \param Path The path pointing to the target audio file
          \sa AudioClip.cs
        */
        public static AudioClip LoadAudioClip(string _path) {
            if (_assetBase.ContainsKey(_path)) {
                if (_assetBase[_path] is AudioClip) {
                    return _assetBase[_path] as AudioClip;
                }
            }

            AudioClip _clip = new AudioClip(AssetPath + _path);
            _assetBase.Add(_path, _clip);
            return _clip;
        }

        //! Reset the asset cash
        /*!
        */
        public static void Reset() {
            _assetBase = new Dictionary<string, IAsset>();
        }
    }
}
