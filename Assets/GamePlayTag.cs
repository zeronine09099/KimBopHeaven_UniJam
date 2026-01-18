using System;
using System.Diagnostics;
using UnityEngine;


[Serializable]
[DebuggerDisplay("{m_Name}")]
public struct GameplayTag : IEquatable<GameplayTag>
{
    // 구분자 정의
    private const char Separator = '.';

    /// <summary>
    /// 빈 태그 (None)
    /// </summary>
    public static readonly GameplayTag None = new GameplayTag(null);

    [SerializeField]
    private string m_Name;

    // 생성자
    public GameplayTag(string name)
    {
        m_Name = name;
    }

    #region Properties

    public readonly bool IsNone => string.IsNullOrEmpty(m_Name);

    public readonly bool IsValid => !string.IsNullOrEmpty(m_Name);

    public readonly string Name => m_Name ?? string.Empty;

    /// <summary>
    /// 부모 태그를 반환합니다. (문자열 파싱으로 계산)
    /// 예: "A.B.C" -> "A.B"
    /// </summary>
    public readonly GameplayTag ParentTag
    {
        get
        {
            if (IsNone) return None;

            int lastDotIndex = m_Name.LastIndexOf(Separator);
            if (lastDotIndex == -1)
            {
                return None; // 점이 없으면 최상위이므로 부모 없음
            }

            return new GameplayTag(m_Name.Substring(0, lastDotIndex));
        }
    }

    // 주의: Definition이 없으므로 Description, Flags, ChildTags 등은 제공 불가
    // public readonly string Description => ... (삭제됨)
    // public readonly ReadOnlySpan<GameplayTag> ChildTags => ... (삭제됨: 매니저 없이 불가능)

    #endregion

    #region Logic

    /// <summary>
    /// 현재 태그가 other의 부모인지 확인
    /// </summary>
    public readonly bool IsParentOf(in GameplayTag other)
    {
        return other.IsChildOf(this);
    }

    /// <summary>
    /// 현재 태그가 parentTag의 자식인지 확인 (StartsWith 문자열 비교)
    /// </summary>
    public readonly bool IsChildOf(in GameplayTag parentTag)
    {
        if (!IsValid || !parentTag.IsValid) return false;
        
        // 1. 부모보다 길이가 짧으면 자식일 수 없음
        if (m_Name.Length <= parentTag.m_Name.Length) return false;

        // 2. 부모 이름으로 시작하는지 확인
        if (!m_Name.StartsWith(parentTag.m_Name, StringComparison.Ordinal)) return false;

        // 3. 경계 확인 (예: "Apple"은 "App"의 자식이 아님. "App.Pie"는 "App"의 자식임)
        return m_Name[parentTag.m_Name.Length] == Separator;
    }

    /// <summary>
    /// 정확히 일치하는지 확인
    /// </summary>
    public readonly bool Matches(in GameplayTag other)
    {
        return Equals(other);
    }

    #endregion

    #region Interfaces & Overrides

    public readonly bool Equals(GameplayTag other)
    {
        return string.Equals(m_Name, other.m_Name, StringComparison.Ordinal);
    }

    public override readonly bool Equals(object obj)
    {
        if (obj is GameplayTag other)
            return Equals(other);

        if (obj is string otherStr)
            return string.Equals(m_Name, otherStr, StringComparison.Ordinal);

        return false;
    }

    public override readonly int GetHashCode()
    {
        return m_Name != null ? m_Name.GetHashCode() : 0;
    }

    public override readonly string ToString()
    {
        return IsNone ? "<None>" : m_Name;
    }

    public static implicit operator GameplayTag(string tagName)
    {
        return new GameplayTag(tagName);
    }

    public static implicit operator string(GameplayTag tag)
    {
        return tag.Name;
    }

    public static bool operator ==(in GameplayTag lhs, in GameplayTag rhs)
    {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(in GameplayTag lhs, in GameplayTag rhs)
    {
        return !lhs.Equals(rhs);
    }

    #endregion
}
