1순위: 코드 정리

MonsterStatus, MonsterAI, StageManager, SpawnManager, BattleManager 역할 분리 확인
이벤트 연결 정리
죽은 몬스터 처리 방식 통일
public 줄이고 [SerializeField] private로 정리

2순위: 포트폴리오용 구조 고정

스테이지 흐름
몬스터 스폰
보상 지급
인벤토리 사용
타겟 탐색

이 흐름이 안정적으로 돌아가게 만들기.

3순위: 새 기능 추가
그 다음에 추가할 만한 건:

아이템 드랍
UI
체력바
스킬 쿨타임
A* 길찾기 표시
보스 패턴

==============
오늘 고친것, PlayerInputHandler을 통하여 입력이 중구난방으로 퍼져있던걸 한곳으로 모아서 관리
사용시 컴포넌트를 붙혀서 input.??pressed 방식으로 사용

다음에 할 일 : 스킬 추가 해보기, UI 연결 , 인벤토리 구체화

1. 코드 자동 정렬
2. README.md로 변경
3. CameraTarget 입력 정리
4. Skill 시스템 시작