using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Parser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)//반환 값이 다이얼로그인 파싱하는 역할의 함수
    {
        List<Dialogue> dialoguesList = new List<Dialogue>(); //(다이얼로그만 들어갈수 있게 리스트자리를 선언후 바로 대사 리스트를 생성) 파싱된 데이터를 입시로 저장할곳
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName); //CSV파일을 받기위한 맞는 데이터구조  TextAsset 변수인 csvData에다가 리소스 폴더에 있는것을 _CSVFileName에서 받아온 경로로 TextAsset형식으로 로드해온다

        string[] data = csvData.text.Split(new char[] { '\n' });//가져온 데이터를 캐릭터 값 배열에다가 엔터단위(\n)로 쪼개서 스트링 값 데이타의 넣기

        for (int i = 1; i < data.Length;)//캐릭터 이름이 있는 1번부터 반복
        {
            string[] row = data[i].Split(new char[] { ',' });//받아온 데이터를 id,이름,대사,로 쪼개기위한 작업(콤마 단위로 쪼개짐)

            Dialogue dialogue = new Dialogue(); // 대사 리스트 생성(임시 저장할 곳이 아닌 진짜로 저장되는곳)

            dialogue.name = row[1];
            List<string> contextList = new List<string>();//다이얼로그의 콘텍스트는 크기가 정해지지않은 배열이라서 로우[2]를 넣을수 없기때문에 리스트를 만들어 그곳에다가 대사 데이터를 저장
            List<string> sproteList = new List<string>();

            do
            {
                contextList.Add(row[2]);//대사는 무조건 실행되어야하므로
                sproteList.Add(row[3]); ;//스프라이트 이름은 엑셀에서 3번째에 위치해있다
                
                if (++i < data.Length)//i를 더했을때 데이터의 렝스값보다 작다면
                {
                    row = data[i].Split(new char[] { ',' });//ID가 그대로일경우 추가된 대사를 똑같이 쪼개는 작업
                }
                else
                {
                    break;//더 반복할 필요가 없으므로 브레이크
                }
            } while (row[0].ToString() == "");//최초로 한번 조건 비교없이 실행하고  이후 조건비교후 반복여부를 판단(이름과 대사를 가져오고 ID가 여백이라면 같은 ID이므로 대사만 가져옴.

            dialogue.conTexts = contextList.ToArray(); //리스트를 배열로 치환시키는 과정(만들어놓은 다이얼로그 context에다가 리스트에 저장된 대사데이터를 치환해서 옮기는 과정.)
            dialogue.spritename = sproteList.ToArray();//가져온 스프라이트 이름을 저장
            dialoguesList.Add(dialogue);//리스트에다가 정보를 대입하는 마무리 과정으로 추정
        }
        return dialoguesList.ToArray();//다이얼로그 배열 타입으로 반환시키기위해서 쓰는 명령어(ToArray = 리스트를 배열로 형변환 하는 명령어)
    }

}
