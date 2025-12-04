# Google Gemini AI 채팅 앱 (.NET 10)

.NET 10을 사용하여 Google Gemini AI 모델과 대화할 수 있는 콘솔 애플리케이션입니다.

## 필수 요구사항

- .NET 10 SDK
- Google API 키 (Gemini API 액세스용)

## Google API 키 발급 방법

1. [Google AI Studio](https://aistudio.google.com/app/apikey)에 접속
2. Google 계정으로 로그인
3. "Create API Key" 버튼 클릭
4. 생성된 API 키 복사

## 설치 및 실행

### 1. 프로젝트 복원
```bash
cd GeminiApp
dotnet restore
```

### 2. API 키 설정

#### 방법 1: 환경 변수 설정 (PowerShell)
```powershell
$env:GOOGLE_API_KEY = "your-api-key-here"
```

#### 방법 2: User Secrets 사용 (권장)
```bash
# User Secrets 초기화
dotnet user-secrets init

# API 키 저장
dotnet user-secrets set "GOOGLE_API_KEY" "your-api-key-here"
```

User Secrets를 사용하는 경우 `Program.cs`를 다음과 같이 수정:
```csharp
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var apiKey = configuration["GOOGLE_API_KEY"];
```

### 3. 애플리케이션 실행
```bash
dotnet run
```

## 사용 방법

1. 프로그램을 실행하면 채팅 인터페이스가 나타납니다.
2. 원하는 질문이나 메시지를 입력하세요.
3. Gemini AI가 응답합니다.
4. 종료하려면 `exit` 또는 `quit`을 입력하세요.

## 예제 대화

```
🤖 Google Gemini AI 채팅 앱
==========================================
종료하려면 'exit' 또는 'quit'을 입력하세요.

당신: 안녕하세요!
Gemini: 안녕하세요! 무엇을 도와드릴까요?

당신: C#에서 비동기 프로그래밍을 설명해주세요.
Gemini: C#의 비동기 프로그래밍은 async/await 키워드를 사용하여...

당신: exit
👋 채팅을 종료합니다.
```

## 주요 기능

- ✅ Google Gemini AI 모델 통합
- ✅ 대화형 채팅 인터페이스
- ✅ 환경 변수 또는 User Secrets를 통한 안전한 API 키 관리
- ✅ 에러 핸들링

## 사용된 패키지

- **Mscc.GenerativeAI** (v2.9.4): Google Gemini API 클라이언트

## 문제 해결

### API 키 오류
```
⚠️ GOOGLE_API_KEY 환경 변수를 설정해주세요.
```
→ API 키가 올바르게 설정되었는지 확인하세요.

### 네트워크 오류
- 인터넷 연결을 확인하세요.
- 방화벽 설정을 확인하세요.

## 라이선스

이 프로젝트는 학습 및 개발 목적으로 자유롭게 사용할 수 있습니다.

## 추가 리소스

- [Google Gemini API 문서](https://ai.google.dev/docs)
- [Mscc.GenerativeAI GitHub](https://github.com/mscraftsman/generative-ai)
- [.NET 10 문서](https://learn.microsoft.com/dotnet/)
