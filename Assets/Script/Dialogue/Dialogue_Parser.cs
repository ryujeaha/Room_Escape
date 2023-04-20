using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Parser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)//��ȯ ���� ���̾�α��� �Ľ��ϴ� ������ �Լ�
    {
        List<Dialogue> dialoguesList = new List<Dialogue>(); //(���̾�α׸� ���� �ְ� ����Ʈ�ڸ��� ������ �ٷ� ��� ����Ʈ�� ����) �Ľ̵� �����͸� �Խ÷� �����Ұ�
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName); //CSV������ �ޱ����� �´� �����ͱ���  TextAsset ������ csvData���ٰ� ���ҽ� ������ �ִ°��� _CSVFileName���� �޾ƿ� ��η� TextAsset�������� �ε��ؿ´�

        string[] data = csvData.text.Split(new char[] { '\n' });//������ �����͸� ĳ���� �� �迭���ٰ� ���ʹ���(\n)�� �ɰ��� ��Ʈ�� �� ����Ÿ�� �ֱ�

        for (int i = 1; i < data.Length;)//ĳ���� �̸��� �ִ� 1������ �ݺ�
        {
            string[] row = data[i].Split(new char[] { ',' });//�޾ƿ� �����͸� id,�̸�,���,�� �ɰ������� �۾�(�޸� ������ �ɰ���)

            Dialogue dialogue = new Dialogue(); // ��� ����Ʈ ����(�ӽ� ������ ���� �ƴ� ��¥�� ����Ǵ°�)

            dialogue.name = row[1];
            List<string> contextList = new List<string>();//���̾�α��� ���ؽ�Ʈ�� ũ�Ⱑ ������������ �迭�̶� �ο�[2]�� ������ ���⶧���� ����Ʈ�� ����� �װ����ٰ� ��� �����͸� ����
            List<string> sproteList = new List<string>();

            do
            {
                contextList.Add(row[2]);//���� ������ ����Ǿ���ϹǷ�
                sproteList.Add(row[3]); ;//��������Ʈ �̸��� �������� 3��°�� ��ġ���ִ�
                
                if (++i < data.Length)//i�� �������� �������� ���������� �۴ٸ�
                {
                    row = data[i].Split(new char[] { ',' });//ID�� �״���ϰ�� �߰��� ��縦 �Ȱ��� �ɰ��� �۾�
                }
                else
                {
                    break;//�� �ݺ��� �ʿ䰡 �����Ƿ� �극��ũ
                }
            } while (row[0].ToString() == "");//���ʷ� �ѹ� ���� �񱳾��� �����ϰ�  ���� ���Ǻ��� �ݺ����θ� �Ǵ�(�̸��� ��縦 �������� ID�� �����̶�� ���� ID�̹Ƿ� ��縸 ������.

            dialogue.conTexts = contextList.ToArray(); //����Ʈ�� �迭�� ġȯ��Ű�� ����(�������� ���̾�α� context���ٰ� ����Ʈ�� ����� ��絥���͸� ġȯ�ؼ� �ű�� ����.)
            dialogue.spritename = sproteList.ToArray();//������ ��������Ʈ �̸��� ����
            dialoguesList.Add(dialogue);//����Ʈ���ٰ� ������ �����ϴ� ������ �������� ����
        }
        return dialoguesList.ToArray();//���̾�α� �迭 Ÿ������ ��ȯ��Ű�����ؼ� ���� ��ɾ�(ToArray = ����Ʈ�� �迭�� ����ȯ �ϴ� ��ɾ�)
    }

}
