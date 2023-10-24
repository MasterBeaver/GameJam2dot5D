using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using Steamworks;
using System;

public class PlayerInfoDisplay : NetworkBehaviour 
{
    [SyncVar(hook = nameof(HandleSteamIdUpdate))]
    private ulong steamId;

  

    [SerializeField] private GameObject obj1;
    [SerializeField] private RawImage profilePix;
    [SerializeField] private TMP_Text Disnaem;





    protected Callback<AvatarImageLoaded_t> avatarImage;

#region Server

    public void SetSteamId(ulong steamId)
    {
        this.steamId = steamId;
    }

    #endregion

    #region Client

    public override void OnStartClient()
    {
        avatarImage = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
    }
    private void HandleSteamIdUpdate(ulong oldSteamId, ulong newSteamId)
    {
        var cSteamId = new CSteamID(newSteamId);

        Disnaem.text = SteamFriends.GetFriendPersonaName(cSteamId);

        int imageId = SteamFriends.GetLargeFriendAvatar(cSteamId);

        if(imageId == -1) { return; }

        profilePix.texture = GetSteamImageAsTexture(imageId);
    }

    private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
    {
        if(callback.m_steamID.m_SteamID != steamId) {return;}

        profilePix.texture = GetSteamImageAsTexture(callback.m_iImage);
    }
    
    private Texture2D GetSteamImageAsTexture(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint hight);
        
        if(isValid)
        {
            byte[] image = new byte[width * hight * 4];

            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * hight * 4));
        
            if(isValid)
            {
                texture = new Texture2D((int)width, (int)hight, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }

        return texture;
    }

#endregion
}
