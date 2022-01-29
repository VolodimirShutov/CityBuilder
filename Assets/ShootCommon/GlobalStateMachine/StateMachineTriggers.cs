namespace ShootCommon.GlobalStateMachine
{
    public enum StateMachineTriggers 
    {
        Start,
        PrivacyPolicyState,
        InitABFState,
        SelectConfigState,
        ReadCachingUserInfoState,
        GetAlternativeConfigsState,
        InitAdServiceState,
        InitIAPState,
        DownloadBaseSourcesState,
        CreatePreloadersState,
        
        LobbyState,
        LobbyMainState,
        LobbySelectLocationState,
        ShopState,
        PVPState,
        OpenChestState,
        CollectionsState,
        
        GamePlayPreloaderState,
        #region Game Play Preloader Substates
        StartSetupGameSubstate,
        CreateInteractiveItemsSubstate,
        InteractiveItemsSetupSubstate,
        #endregion
        
        MainGamePlayState,
        #region Main Game Play Substates
        MainGameSubstate,
        PauseGameSubstate,
        AddTimeGameSubstate,
        TatgetsShowGameSubstate,
        BuyInnerBoosterGameSubstate,
        #endregion

        #region Rooms
        StartRoomsSubstate,
        CreateRoomInteractiveItemsSubstate,
        PlayStartRoomsAnimationSubstate,
        RoomInteractiveItemsSetupSubstate,
        RoomMainGamePlaySubstate,
        NextRoomPlaySubstate,
        #endregion
        
        LoseGameState,
        WinGameState,
        EndGameState,
        
        #region Shop
        BuyOfferGameSubstate,
        #endregion
    }
}