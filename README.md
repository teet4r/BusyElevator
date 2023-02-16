# Project: 분주한 승강씨

</br>

## 1. 개요
사람들의 요구에 맞게 엘리베이터를 적절하게 운용하여 50일을 버텨야 하는 모바일 2D 게임

</br>

## 2. 제작기간 & 참여인원
- 약 2달 소요
- 기획 겸 프로그래머 2명

</br>

## 3. 사용언어 & 도구
- C#
- Visual Studio
- Unity 2D(에디터 버전: 2021.3.5f1)

</br>

## 4. 구동 플랫폼
- Android

</br>

## 5. 주요 구현 이슈
<details>
<summary>오브젝트 풀링</summary>
<div markdown="1">

- 사람 오브젝트를 풀링하여 Instantiate & Destroy 비용 최소화

</div>
</details>

<details>
<summary>사람 오브젝트</summary>
<div markdown="1">

- Human 클래스를 만들어 일반 사람, 뚱뚱한 사람, 그 밖의 다양한 타입의 사람 오브젝트가 상속받을 수 있도록 함
- ActInterface를 구현하여 사람 오브젝트 간에 상호작용이 가능하도록 함(도둑은 가드에게 잡히면 사라짐 등)
- 층별로 존재하는 이벤트 오브젝트(자판기, 문, ATM)와도 상호작용 가능

</div>
</details>

<details>
<summary>층 오브젝트</summary>
<div markdown="1">

- 일정한 시간이 지나면 랜덤으로 층이 생성
- 거주 층, 병원 층, 편의점 층 다양하게 구성
- Human 클래스와 마찬가지로 공통 기능이 모여있는 Floor 클래스를 바탕으로 층 생성


</div>
</details>

<details>
<summary>버프 및 디버프</summary>
<div markdown="1">

- 일정 시간마다 선택 가능한 버프가 세개씩 등장
- 사람 및 엘리베이터, 층의 기능에 영향을 주는 버프와 랜덤으로 무조건 하나씩 받아야 하는 디버프 존재

</div>
</details>

<details>
<summary>UI</summary>
<div markdown="1">

- 화면 상단에는 체력 창이 존재하여 사람들이 엘리베이터를 타지 못했을 때마다 조금씩 차감
- speed 버튼 및 pause 버튼은 Time.TimeScale을 이용하여 조절
- 경과한 일 수, 현재 시간, 가지고 있는 재화 및 엘리베이터 대기자 수 등으로 현재 상황 표시
- 화면 중앙 및 하단에 건물, 직원, 장비 창을 통해 관리 및 업그레이드 가능

</div>
</details>

</br>

## 6. 스크린샷
<details>
<summary>메인화면</summary>
<div markdown="1">

![BusyElevator Main0](https://user-images.githubusercontent.com/76508241/219283451-3341e777-5e25-4eda-a76a-d244c1a5850a.jpg)

</div>
</details>

<details>
<summary>인게임</summary>
<div markdown="1">

![BusyElevator Ingame0](https://user-images.githubusercontent.com/76508241/219283458-2bc6a558-508f-494a-823e-62a19aaf9a28.jpg)
![BusyElevator Ingame1](https://user-images.githubusercontent.com/76508241/219283461-4544eb6c-1d20-4f02-a564-4198582df709.jpg)
![BusyElevator Ingame2](https://user-images.githubusercontent.com/76508241/219283465-99b181f7-9182-4c07-ae9a-04648e9b25ab.jpg)
![BusyElevator Ingame3](https://user-images.githubusercontent.com/76508241/219283466-d431bcd3-da6c-410f-9c18-1c6e4590d8be.jpg)
![BusyElevator Ingame4](https://user-images.githubusercontent.com/76508241/219283468-13e99d45-558b-4464-b5b3-f478971bf7d9.jpg)
![BusyElevator Ingame5](https://user-images.githubusercontent.com/76508241/219283471-2d9fd02d-3409-4b12-b6b9-bfd7fa341062.jpg)

</div>
</details>

<details>
<summary>게임오버</summary>
<div markdown="1">

![BusyElevator End0](https://user-images.githubusercontent.com/76508241/219283473-cfe3780b-f8bb-4f99-a5cf-96ce16c04e4d.jpg)

</div>
</details>

</br>

## 7. 링크
- [플레이 영상 및 다운로드](https://drive.google.com/drive/folders/1a2FGpd-uSt1XSp1vml1UQKZhc5BCbHHU)
