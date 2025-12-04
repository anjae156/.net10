var builder = WebApplication.CreateBuilder(args);

// 개발 중 상세한 에러 확인을 위해 예외 페이지 활성화
builder.Services.AddProblemDetails();

// 닉네임 생성 서비스 등록
builder.Services.AddSingleton<NicknameGenerator>();

// --- 웹 애플리케이션 구성 ---

var app = builder.Build();

// 예외 처리 미들웨어 추가
app.UseExceptionHandler();
app.UseStatusCodePages();

// GET /api/nickname 엔드포인트 정의
app.MapGet("/api/nickname", async (NicknameGenerator generator, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("닉네임 생성 요청 시작");
        
        var nickname = generator.Generate();
        
        logger.LogInformation("닉네임 생성 성공: {Nickname}", nickname);
        return Results.Ok(new { nickname });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "닉네임 생성 중 오류 발생");
        return Results.Problem(
            detail: ex.Message, 
            statusCode: 500,
            title: "닉네임 생성 실패");
    }
});

app.Run();

// 로컬 AI 기반 닉네임 생성기
public class NicknameGenerator
{
    private readonly Random _random = new();
    
    // 재미있는 형용사
    private readonly string[] _adjectives = 
    {
        // 감정
        "행복한", "슬픈", "화난", "졸린", "배고픈", "신나는", "지친", "용감한",
        "똑똑한", "귀여운", "무서운", "웃긴", "시끄러운", "조용한", "빠른", "느린",
        
        // 상태/특징
        "뜨거운", "차가운", "밝은", "어두운", "투명한", "반짝이는", "빛나는", "어둠의",
        "전설의", "운명의", "불멸의", "영원한", "순간의", "찰나의", "무한의", "최강의",
        "최약의", "궁극의", "절대적", "상대적", "환상의", "현실의", "꿈꾸는", "날아가는",
        
        // 유머러스
        "폭발하는", "녹아내리는", "썩어가는", "발효된", "튀김", "구운", "삶은", "찐",
        "얼어붙은", "불타는", "짜릿한", "달콤한", "쌉싸름한", "매운", "신", "짠",
        "비싼", "싼", "공짜", "사기", "사기친", "거짓말하는", "진실된", "솔직한",
        
        // 게임/판타지
        "암흑의", "빛의", "그림자", "황금", "은빛", "다이아몬드", "강철", "나무",
        "불의", "물의", "바람의", "땅의", "번개의", "얼음의", "독의", "성스러운"
    };
    
    // 재미있는 명사
    private readonly string[] _nouns = 
    {
        // 귀여운 동물
        "고양이", "강아지", "토끼", "햄스터", "앵무새", "거북이", "금붕어", "병아리",
        "오리", "펭귄", "코알라", "판다", "알파카", "라마", "수달", "미어캣",
        
        // 웃긴 동물
        "슬라임", "젤리", "푸딩", "두부", "곤약", "묵", "떡", "송편",
        "만두", "교자", "딤섬", "완탕", "군만두", "찐빵", "호빵", "붕어빵",
        
        // 강한 존재
        "용", "드래곤", "호랑이", "사자", "곰", "늑대", "독수리", "매",
        "전사", "마법사", "궁수", "도적", "암살자", "기사", "팔라딘", "사제",
        "닌자", "사무라이", "검사", "창병", "마왕", "용사", "현자", "예언자",
        
        // 음식
        "치킨", "피자", "햄버거", "떡볶이", "라면", "짜장면", "짬뽕", "탕수육",
        "김치", "순대", "튀김", "돈가스", "초밥", "라멘", "우동", "소바",
        "케이크", "도넛", "쿠키", "마카롱", "타르트", "푸딩", "젤리", "사탕",
        
        // 이상한 것들
        "변기", "휴지", "똥", "방귀", "콧물", "침", "땀", "귀지",
        "발톱", "손톱", "털", "비듬", "먼지", "쓰레기", "재떨이", "걸레",
        
        // 사물
        "컴퓨터", "키보드", "마우스", "모니터", "USB", "하드", "SSD", "RAM",
        "CPU", "GPU", "냉장고", "에어컨", "선풍기", "청소기", "세탁기", "건조기",
        
        // 자연
        "태풍", "번개", "천둥", "지진", "화산", "쓰나미", "토네이도", "폭풍",
        "별", "달", "해", "혜성", "유성", "블랙홀", "은하", "우주",
        
        // 랜덤
        "폭탄", "미사일", "로켓", "UFO", "외계인", "좀비", "유령", "뱀파이어",
        "늑대인간", "미라", "프랑켄슈타인", "드라큘라", "악마", "천사", "요정", "마녀"
    };
    
    // 웃긴 접미사
    private readonly string[] _suffixes = 
    {
        "", "", "", // 없는 경우가 많도록
        "님", "왕", "신", "쟁이", "러", "스터", "킹", "퀸",
        "요정", "천사", "악마", "전사", "고수", "프로", "장인", "달인",
        "마스터", "제왕", "황제", "대왕", "대마왕", "용사", "영웅", "전설",
        "찐따", "느님", "갓", "God", "ㅋㅋ", "ㅎㅎ", "ㄷㄷ", "ㅜㅜ",
        "맨", "우먼", "보이", "걸", "베이비", "키드", "오빠", "언니",
        "형", "누나", "아저씨", "아줌마", "할배", "할매", "꼰대", "라떼"
    };
    
    // 특수 접두사 (가끔 추가)
    private readonly string[] _prefixes = 
    {
        "", "", "", "", "", // 대부분 없음
        "진짜", "가짜", "찐", "헛", "거짓", "참",
        "슈퍼", "울트라", "하이퍼", "메가", "기가", "테라",
        "미니", "작은", "큰", "거대한", "초소형", "초대형"
    };
    
    public string Generate()
    {
        var style = _random.Next(8); // 8가지 스타일
        
        return style switch
        {
            0 => GenerateBasic(),           // 형용사 + 명사
            1 => GenerateWithNumber(),      // 형용사 + 명사 + 숫자
            2 => GenerateWithSuffix(),      // 형용사 + 명사 + 접미사
            3 => GenerateDouble(),          // 명사 + 명사
            4 => GenerateComplex(),         // 형용사 + 명사 + 접미사 + 숫자
            5 => GenerateFunny(),           // 접두사 + 명사 + 접미사
            6 => GenerateTriple(),          // 형용사 + 명사 + 명사
            _ => GenerateCrazy()            // 완전 랜덤 조합
        };
    }
    
    private string GenerateBasic()
    {
        return $"{_adjectives[_random.Next(_adjectives.Length)]}{_nouns[_random.Next(_nouns.Length)]}";
    }
    
    private string GenerateWithNumber()
    {
        var adj = _adjectives[_random.Next(_adjectives.Length)];
        var noun = _nouns[_random.Next(_nouns.Length)];
        var number = _random.Next(1, 99999);
        return $"{adj}{noun}{number}";
    }
    
    private string GenerateWithSuffix()
    {
        var adj = _adjectives[_random.Next(_adjectives.Length)];
        var noun = _nouns[_random.Next(_nouns.Length)];
        var suffix = _suffixes[_random.Next(_suffixes.Length)];
        return $"{adj}{noun}{suffix}";
    }
    
    private string GenerateDouble()
    {
        var noun1 = _nouns[_random.Next(_nouns.Length)];
        var noun2 = _nouns[_random.Next(_nouns.Length)];
        return $"{noun1}{noun2}";
    }
    
    private string GenerateComplex()
    {
        var adj = _adjectives[_random.Next(_adjectives.Length)];
        var noun = _nouns[_random.Next(_nouns.Length)];
        var suffix = _suffixes[_random.Next(_suffixes.Length)];
        var number = _random.Next(2) == 0 ? _random.Next(1, 9999).ToString() : "";
        return $"{adj}{noun}{suffix}{number}";
    }
    
    private string GenerateFunny()
    {
        var prefix = _prefixes[_random.Next(_prefixes.Length)];
        var noun = _nouns[_random.Next(_nouns.Length)];
        var suffix = _suffixes[_random.Next(_suffixes.Length)];
        return $"{prefix}{noun}{suffix}";
    }
    
    private string GenerateTriple()
    {
        var adj = _adjectives[_random.Next(_adjectives.Length)];
        var noun1 = _nouns[_random.Next(_nouns.Length)];
        var noun2 = _nouns[_random.Next(_nouns.Length)];
        return $"{adj}{noun1}{noun2}";
    }
    
    private string GenerateCrazy()
    {
        var prefix = _prefixes[_random.Next(_prefixes.Length)];
        var adj = _adjectives[_random.Next(_adjectives.Length)];
        var noun = _nouns[_random.Next(_nouns.Length)];
        var suffix = _suffixes[_random.Next(_suffixes.Length)];
        var number = _random.Next(3) == 0 ? _random.Next(1, 9999).ToString() : "";
        
        // 가끔 이상한 조합
        var parts = new List<string>();
        if (!string.IsNullOrEmpty(prefix)) parts.Add(prefix);
        if (_random.Next(2) == 0) parts.Add(adj);
        parts.Add(noun);
        if (!string.IsNullOrEmpty(suffix)) parts.Add(suffix);
        if (!string.IsNullOrEmpty(number)) parts.Add(number);
        
        return string.Join("", parts);
    }
}

