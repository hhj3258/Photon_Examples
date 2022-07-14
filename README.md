## 게임 기안
* ### 게임 소개
  **SF 장르를 내세운 다양한 무기와 빠른 속도감의 2D 플래포머 팀 게임**

![image](uploads/a93c71af2da608de3898922ec99f1dfe/image.png)

* ### 장르
  2D 플래포머, 팀전, SF, 난투, 멀티플레이, 슈팅

* ### 핵심 재미
  속도감, 난투, 시야 제약, 슈퍼 무기

* ### 목표
  * 실시간 멀티플레이 게임이라는 특성상 정확한 네트워크 동기화 처리 구현
  * 처음 접하는 유저라도 진입장벽이 낮은 직관적인 플레이 방식
  * 빠른 속도감의 난투를 살린 게임 디자인
  * 화려한 이펙트 및 시각적인 효과 표현



## 구현 목표
* ### 전체
  * 이동 로직(구르기, 로켓부츠, 경사면에 따른 속도 변화)
  * 발사체 로직(탄환 낙차 유무, 대미지, 속도)
  * AI(복잡한 지형, 플레이어 추적, 에임 정확도)
  * 멀티플레이(실시간, 빠른 속도, 동기화)

</br>

* ### 주무기 - 밀리터리
  * 현실 밀리터리 웨폰(ex. ak74, 미니건, m4 ...)
  * 총알 낙차 구현
  * 무기별 대미지 조절을 위한 json, csv, 스크립터블 오브젝트 등과 같은 데이터 테이블 적용
  * 종류 늘리는 부분은 총알 대미지, 연사 속도 등의 조정으로 하기 때문에 오래 걸리지 않을 예정

</br>

* ### 슈퍼무기 - SF
  * 플래그 팀전을 염두에 두고 슈퍼무기 디자인
  * 공격 팀에게 필요한 슈퍼무기, 수비 팀에게 필요한 슈퍼무기
  * 팀전 특성상 슈퍼무기에 대미지가 있을 필요는 없음
  * 슈퍼무기 스폰 시나리오
    * case1) 각자의 진영에 스폰
      * 20~30초에 한 번 스폰
      * 스폰포인트에는 플레이어가 없어야 함
    * case2) 맵 중앙에 스폰
      * 카운트 다운 필요(플레이어들 유도)
    * case3) 모든 플레이어가 1분마다 선택 가능
      * 슈퍼무기 선택 후 플레이어가 사망하면 그 시점부터 쿨타임 1분 후 다시 선택 가능

  무기 이름 | 컨셉 | 발사 방식 | 발동 방식 | 지속 시간 | 예시
  -----------|------|------------|-----------|------------|------
  순간이동 장치  | 특정 장소로 순간이동 | 수류탄   | 버튼 한 번 더 누르기 | N 초 뒤 소멸        | 나루토 미나토 수리검
  터렛 생성 장치 | 자동 공격 터렛 설치   | 수류탄   | 지형에 닿았을 때     | 내구도 0 or 탄환수 0 | 오버워치 토르비욘
  중력자 탄       | 범위 안의 적 끌어당김 | 수류탄  | 지형에 닿았을 때     | N 초 뒤 소멸        | 오버워치 자리야 궁극기
  EMP 탄          | 이동 외 모든 행동 불가 | 수류탄 | 오브젝트 닿았을 때 | N 초 지속             | 롤 침묵 스킬
  쉴드             | 대미지 방어              | X         | 누른 순간 발동      | N초 지속, 내구도 0 | 롤 뽀삐 실드
  추가 예정

</br>

* ### 시스템(인게임 룰)
  * 기본 룰
      * 플레이 타임: 5 ~ 10 분
      * 낮밤 사이클: 낮-밤 1 사이클 기준 2번 반복
      * 플레이어 수: 최대 16명(N명 플레이어, 16-N명 봇)
      * 부활 시 주무기 선택 가능
      * 주무기 탄창은 무제한(재장전 무한히 가능)
      * 슈퍼무기는 맵 상 특정 지점에 스폰
      * 플레이어 사망 시 구급상자, 탄약 드랍
     

  * ### 플래그 팀전
    * 상대방 진영의 플래그를 내 진형으로 가지고 오는 방식
    * 맵 중앙에서 리스폰 되는 미니언(시간대 별 아군 적군 달라짐)
    * Day 팀 vs Night 팀
    * Day 팀은 '밤' 시간에 시야 제약(디버프)
    * Night 팀은 '낮' 시간에 시야 제약(디버프)
    * ex) 낮 시간
      * Day 팀이 공격에 나서도록 게임 디자인
      * Night 팀은 낮 시간에 시야제약을 받음 -> 수비
    * ex) 밤시간
      * 낮시간과 반대(Night 팀 공격, Day 팀 수비)
    * 각 진영에는 수비를 위한 기관총이 설치되어 있음

</br>

  * ### 시야 제약
    * 화살표 UI(플레이어 위치)
    * 화살표: 모든 플레이어 표시
    * 카메라를 줌인 시킨다.(몰입감 증가)
    * 총알 궤적, 총구 화염 보임.
    * 맵(지형) 보임
    * 내 진영, 상대 진영의 플래그 보임
    * 시야 제약이 발동되는 연출을 세련되게 표현할 필요가 있음.
    * ![image](uploads/5f6280e1e3dc3371ad04557b7ef34776/image.png)


## 구현 이슈
- 총알 등 빠른 속도를 가진 발사체의 정확한 충돌 처리 필요
- 애니메이션: 자연스럽고 역동적인 애니메이션 지향, 래그돌 & IK 적용 가능 여부 확인 필요
- 밀리터리 총, SF 무기 구성 로직
- 봇(AI) 로직



## 계획
  * ### 플랜 - 프로젝트 기간에 따라 아래 사항들을 조정
    * A 피쳐리스트의 모든 항목 구현
    * B AI 단순화 , 보조 무기 항목 조정, 밤낮 삭제
    * C 킬캠 삭제, 보조 무기 항목 조정, 미니언 삭제

</br>

  * ### [피쳐 리스트 & 마일스톤](https://docs.google.com/spreadsheets/d/1sG2zR_o5OnRKNkRjh9_TFjdaFMrJmM-rIhLWZfiX_1M/edit?usp=sharing)



## 업무 분담
이름 | 역할
-----|-----
박종화 | 팀장
곽진성 | 개발
김정홍 | 개발
홍현준 | 개발



## 사업성

Project Brawl은 난투 컨셉을 메인으로 한 원초적 재미를 추구하는 게임입니다.

2022 게임 이용 행태 및 인식 설문조사에서 MZ 세대 설문자들이 중요하게 생각하는 가치는 다음과 같았습니다.

![image](uploads/6dfa889da7dd6f92a1866b5ddac11532/image.png)
 
저희는 게임 플레이의 타겟 층을 MZ 세대에 맞춰 기본 컨셉인 원초적 전투를 통해 승리하는 성취감을 부여하는 것뿐만 아니라, 낮과 밤의 시간 시스템과 다양한 SF 컨셉의 보조 무기 등의 다양한 콘텐츠 추가를 통해 게임의 재미를 높일 예정입니다.

수익 모델은 패키지 게임으로 제작하여 스팀 플랫폼에 출시를 하여 판매 수익을 얻는 것과 게임 내 캐릭터의 유료 스킨을 판매하는 것 2가지로 생각하고 있습니다.
      
![image](https://user-images.githubusercontent.com/70702088/178957862-3febbbd4-33cb-479c-bb89-195edae122f0.png)














---

* ### 게임 진행 방식 - 데스매치
  * 승리조건: case1) N 시간동안 M 킬, case2) N 시간동안 모스트 킬
  * 킬 스코어는 죽인 플레이어가 얻는다.(ex. 배틀그라운드)
  * 밤 시간동안은 킬 스코어를 낮 시간보다 N 배 더 얻는다.(1<N)
  * 맵 드랍 아이템
    * 구급상자(피 회복)
    * 탄창(ex. 10/30 일 때 20/30 으로 바로 장전됨)
    * 수류탄

* ### 낮-밤
  * 사이클: 낮-밤 1 사이클 기준 N번 반복
  * 밤 시간 시야제약
    * 무슨 방식으로?

* ### 총기 로직
  * 재장전은 무제한으로 가능
  * 현재 탄창의 총알 갯수는 제한 있음
  * 발사(발사 - 발사 대기시간 - 발사)
  * 재장전(재장전 시작 - 재장전 - 재장전 완료)
    * 1. 재장전 키를 누르면: 재장전
    * 2. 총알이 없을 때 발사 버튼 누른다면: 재장전

* ### 총알 로직
  * 총기 발사 버튼 클릭 시 마우스 에임의 위치로 발사됨
  * 발사 위치 공식(세부 조정 필요): 에임 position * 총기 반동 * 연사 반동
  * 벽을 맞을 경우: N 각도로 맞을 경우 비탄성 충돌, 정반사

* ### 조작법
  * 키보드, 마우스
  * 캐릭터 조작 - 키보드
    * 상하좌우 이동
    * 점프
    * 무기 스왑
    * 로켓부츠(fly)
    * 구르기
    * 앉기
    * 눕기
  * 에임 조작 - 마우스
    * 에임: left button 포지션
    * 발사: left button 클릭

* ### 로켓부츠

* ### 봇(AI)


## UI 스토리보드
![image](https://user-images.githubusercontent.com/70702088/178957939-25b0840b-3f64-403a-a36a-b9045f154c4b.png)
