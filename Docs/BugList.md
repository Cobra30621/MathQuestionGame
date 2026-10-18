# 數學魔法使 Bug 清單

2026-10-15 · 線上版：https://claude.ai/code/artifact/33a588db-0ed7-4a78-ae4b-c9b2e20910c3（狀態以線上版為準）

這份清單是從程式碼找出來的，**都還沒有在遊戲中實際重現**。重現步驟是依程式碼推論的，第一次修之前要先照著做一次，確認問題真的存在。目前沒有 Sentry 紀錄或玩家回報可以對照；拿到之後再補進來，並調整優先順序。

## 總覽

| 編號 | 問題 | 嚴重度 | 修正難度 | 建議用途 | 狀態 |
| --- | --- | --- | --- | --- | --- |
| B1 | 打到已死亡的敵人時，效果佇列停住，戰鬥卡死 | 嚴重 | 中等 | 場次 3：你示範 | 待重現 |
| B2 | 戰鬥中途離開，下一場戰鬥可能卡住 | 嚴重 | 困難 | 階段 3：困難題 | 待重現 |
| B3 | 非「子由」出版社又沒網路時，答題畫面卡住 | 嚴重 | 簡單 | 場次 4：帶著修 | 待重現 |
| B4 | 回合開始時馬上按「結束回合」，敵人回合仍能拖曳手牌 | 中 | 中等 | 階段 3：中等題 | 待重現 |
| B5 | 題目下載超時後，背景仍無限重試 | 中 | 中等 | 階段 3：中等題（備用） | 待重現 |
| B6 | Android 直接滑掉 App 時可能沒存檔 | 中 | 簡單 | 階段 3：簡單題 | 待重現 |
| B7 | 營火「回血」按鈕文字少一個右括號 | 低 | 簡單 | 場次 4 暖身 | 待重現 |
| B8 | 切換場景時畫面凍結（效能） | 低 | 中等 | 之後再處理 | 待重現 |

嚴重度：「嚴重」是遊戲卡死、只能重開；「中」是操作錯誤或資料可能遺失；「低」是顯示問題或效能。狀態：待重現、已重現、修正中、已修好、無法重現。

## B1：打到已死亡的敵人時，效果佇列停住，戰鬥卡死

**現象**：連續打出攻擊卡，前一張把敵人打死後，後面的卡還會打向那隻敵人。這時效果佇列會永遠停住：按「結束回合」後敵人不會行動，戰鬥卡死。因為佇列掛在常駐的 GameManager 上，回主選單再開新戰鬥也一樣卡，只能重開 App。

**重現步驟（推論）**

1. 開 `Test Template 1`，開發者測試器選戰士，敵人選一隻樹精（21 血）。
2. 第 1 回合對樹精打一張揮劍（剩 14 血），結束回合。
3. 第 2 回合**快速**連打三張揮劍到同一隻樹精：第二張打死牠，第三張打向已被銷毀的物件。
4. Console 應出現 `MissingReferenceException`，之後按「結束回合」，敵人回合不會開始。

**位置**

- `Assets/Scripts/Effect/Sequence/FXSequence.cs`，`Execute`：特效位置設為「每一個作用目標」時，直接讀取 `target.transform`，沒有先檢查目標是否還存在。所有正式卡都是這個設定。
- `Assets/Scripts/Effect/Sequence/SequenceManager.cs`，`ProcessSequenceQueue`：任何一個序列丟出例外，整個 coroutine 就中止，`_isExecuting` 會一直停在 `true`。
- `Assets/Scripts/Combat/CombatManager.cs`：敵人回合開始前會一直等 `!EffectExecutor.Instance.IsExecuting`，所以會永遠等下去。

**原因**：真正的問題不是少一個 null 檢查，而是「任何一個效果出錯，整個佇列就再也不會往下跑」。`DelaySequence` 有 try/catch，`FXSequence` 沒有。

**修正方向**

- `FXSequence` 跳過已銷毀的目標。
- 不論序列成功或失敗，都要讓佇列往下跑。例如 `SequenceManager` 確保 `_isExecuting` 一定會被重設，`FXSequence` 出錯時也要呼叫完成回呼。
- 只加 null 檢查也能讓這個重現步驟不卡，但下一個沒想到的例外還是會卡死。**這正是場次 3 要示範的「AI 只加 null 檢查」陷阱。**

## B2：戰鬥中途離開，下一場戰鬥可能卡住

**現象（推論）**：戰鬥中透過選單離開單局遊戲（`ExitSingleGame`）回主選單，再繼續遊戲進入下一場戰鬥時，可能卡住或噴錯。

**位置**

- `Assets/Scripts/Managers/GameManager.cs`，`ExitSingleGame`：離開時沒有清空效果佇列。
- `Assets/Scripts/Effect/Sequence/SequenceManager.cs`：佇列沒有清空的方法。
- `Assets/Scripts/Effect/Damage/DamageEffect.cs`：多段傷害的 coroutine 跑在常駐的 EffectExecutor 上，換場景後還會繼續跑。
- `Assets/Scripts/Combat/CombatCounter.cs`：只在勝利或失敗時解除事件訂閱，中途離開時不會解除，舊的計數器會留在靜態事件上。

**原因**：戰鬥的暫存狀態放在跨場景常駐的物件上，但只有「正常打完」才會清理。

**修正方向**：在離開戰鬥場景時統一清理，包括清空效果佇列、停止 EffectExecutor 上的 coroutine、解除 CombatCounter 的訂閱。建議先修 B1，因為 B1 修好後，這裡的例外也不會再卡死佇列。

**為什麼算困難**：要先理解哪些物件跨場景常駐（GameManager）、哪些會隨場景銷毀（CombatManager），而且重現需要掌握時機。

## B3：非「子由」出版社又沒網路時，答題畫面卡住

**現象**：答題設定選翰林、康軒或南一，沒有網路（或下載逾時）時，畫面會停在答題介面，沒有題目也無法離開。

**重現步驟**

1. 主選單 → 答題設定，出版社選「翰林」，年級任選。
2. 關閉 Wi-Fi 與行動網路（編輯器中可拔網路線，或關閉電腦網路）。
3. 開始答題，Console 應出現 `ArgumentOutOfRangeException`。

**位置**

- `Assets/Data/Questions/Local Question Data.asset`：本機題庫只有「子由」出版社的題目，共 12 個年級各一組。
- `Assets/Scripts/Question/QuestionSession.cs`，`GetNextQuestion`：題目清單是空的時候，沒有檢查就直接取第 0 題。

**修正方向**：本機也沒有題目時，顯示「沒有網路，無法答題」並離開答題，或改用子由的題目。要用哪一種，請和負責人確認。

**適合場次 4 的原因**：一定能重現、Console 的錯誤訊息直接指到出錯的那一行，原因也容易用一句話說清楚。

## B4：回合開始時馬上按「結束回合」，敵人回合仍能拖曳手牌

**現象（推論）**：玩家回合一開始、回合開始的提示動畫還在播放時就按「結束回合」，進入敵人回合後，手牌仍然可以拖曳。

**位置**：`Assets/Scripts/Combat/CombatManager.cs`，`AllyTurnRoutine`；`Assets/Scripts/Combat/CombatCanvas.cs`，`Update`。

**原因**：狀態一切到 `AllyTurn`，「結束回合」按鈕就能按了，但 `AllyTurnRoutine` 還在等提示播完。玩家這時按下結束回合，等提示播完後，這段 routine 仍會執行 `EnableDragging()` 與 `CanSelectCards = true`，把敵人回合的手牌重新打開。

**修正方向**：等待結束後先確認狀態仍是 `AllyTurn` 再開放手牌，或讓按鈕在 `CanSelectCards` 為 true 時才能按。

## B5：題目下載超時後，背景仍無限重試

**現象（推論）**：網路不穩時，答題會在逾時後改用本機題目，但背景的下載不會停止，會一直打 API。每答一次題就多一個背景下載，可能造成手機卡頓與耗電。

**位置**

- `Assets/Scripts/Question/QuestionGenerate/QuestionGenerator.cs`，`DownloadOnlineQuestionsWithTimeout`：逾時後沒有停止下載。
- `Assets/Scripts/Question/QuestionLoader/OnlineQuestionDownloader.cs`：`while (questions.Count < totalQuestionCount)` 沒有重試上限。
- `Assets/Scripts/Question/QuestionLoader/QuestionDownloader.cs`：API 回傳空的題目陣列時，`data.problem[0]` 會丟出例外；`UnityWebRequest` 也沒有設定 timeout。

**修正方向**：逾時就 `StopCoroutine`、設定重試次數上限、為 request 設定 timeout，並在取第 0 題前檢查陣列長度。

## B6：Android 直接滑掉 App 時可能沒存檔

**現象（推論）**：在 Android 上直接從最近使用清單滑掉 App，可能遺失上次存檔後的進度。

**位置**：`Assets/Scripts/Save/SaveWhenLeaveGame.cs` 只在 `OnApplicationQuit` 存檔。Android 上 App 被系統或玩家關掉時，這個事件不一定會觸發。

**修正方向**：在 `OnApplicationPause(true)` 時也存檔。

**待確認**：地圖節點完成時（`EncounterManager.OnRoomCompleted`）已經會存單局資料，所以實際會遺失哪些資料，要先確認。例如主選單答題拿到的寶石是什麼時候存的。

## B7：營火「回血」按鈕文字少一個右括號

**現象**：營火的回血按鈕顯示「回血 (23」，少了右括號。

**位置**：`Assets/Scripts/CampFire/CampFireCanvas.cs`，`OpenCanvas`：`healText.text = $"回血 ({healAmount}";`。

**修正方向**：補上右括號。另外，回血量是用角色資料的初始最大血量計算，不是目前的最大血量；如果之後會有增加最大血量的效果，這裡也要一起改。

**用途**：場次 4 的暖身題，用來練習完整走一次「重現 → 找位置 → 修改 → 開 PR」。

## B8：切換場景時畫面凍結

**現象（推論）**：在手機上切換場景（地圖 → 戰鬥等）時，淡出後畫面會停頓一下。

**位置**：`Assets/Scripts/UI/UIManager.cs`，`ChangeScene` 使用同步的 `SceneManager.LoadScene`。

**修正方向**：改用 `LoadSceneAsync`，淡入淡出要等載入完成。屬於效能問題，等穩定性問題處理完再做。

## 不在這份清單的問題

- 配置表的資料錯誤，例如機器士兵難度三的攻擊、通用卡「機會」參數為 `#VALUE!`，見 [數值平衡手冊](BalanceHandover.md) 的「注意事項」。
- 程式整理類項目，例如題庫 Token 寫死、未使用的套件，見 [Handover.md](Handover.md) 的「已知問題與建議」。
