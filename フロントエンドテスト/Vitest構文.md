# Vitest(Jest)構文
## 単体、結合共通で使う構文
### describe
テストスイートをグループ化するために使用される。関連するテストケースを一つのブロック内にまとめることができる
### it/test
個々のテストケースを定義します。テストしたい具体的な動作や機能に対して記述する。一般的に、it と test は同じ目的で使用されますが、テストケースを記述する際の文脈によって選択する。ケース名が日本語の場合はtestの方が無難な気がする
### beforeAll
すべてのテストケースが実行される前に一度だけ実行される。テストに必要な初期化処理などを行う
### afterAll
すべてのテストケースの実行が完了した後に一度だけ実行される。テストの後片付けなどを行う
### beforeEach/afterEach
それぞれのテストケースが実行される前後に毎回実行される

```
import { describe, it, expect, beforeAll, afterAll } from 'vitest';

describe('数値のテスト', () => {

  beforeAll(() => {
    // すべてのテストケースの前に実行される処理
    console.log('テスト開始前の初期化');
  });

  afterAll(() => {
    // すべてのテストケースの後に実行される処理
    console.log('テスト完了後の後片付け');
  });

  it('数値の等価性テスト', () => {
    expect(2 + 2).toBe(4);
  });

  it('数値の不等価性テスト', () => {
    expect(2 + 2).not.toBe(5);
  });

});

```
## テストで使う構文
### 実行
- カスタムHookのテストをするとき
	- renderHookを使用し対象のHookを個別にテストできるようにする
		```
		const { result } = renderHook(() => useCounter())
		```
	- 戻り値のresultの中の currentにフックの現在の戻り値が入っている
	  基本的には、result.currentのオブジェクトのテストしたいメソッドを呼び出し結果を検証する
	  ```
	  const { result } = renderHook(() => useCounter());
	  /// このテストではuseCounterのcountメソッドに対してテストをしている
	  expect(result.current.count).toBe(0);
	  ```
	- actによって状態を固定してから結果を確認する
	  状態更新が行われずに結果が検証される可能性があるためactによって状態更新を行ったのち結果検証を実施する
	```
	const { result } = renderHook(() => useCounter());  
  
	act(() => {  
	result.current.increment();  
	});  
	  
	expect(result.current.count).toBe(1);
	```
- コンポーネントをテストするとき
	- Renderを使用しコンポーネントをテストする
	```
	import { render } from "@testing-library/react";
	import { Form } from "./Form";
	
	test("名前の表示", () => {render(<Form name="taro" />);}); 
	```
	- コンポーネントのpropsに定義されているものはテストケースに含める
	  テスト対象のふるまいに関与するため。逆に関与しないのであれば、不要なものな可能性が高い。
	  子コンポーネントに渡しているだけの場合でも、正しく渡せているかは確認すべき
### 結果検証
- expect
	アサーション
- 値取得
	- コンポーネントのテストではDOM構造から結果を確認するのではなく、ユーザーがどう認識、操作するか(ふるまい)で要素を取得する
	  そのためDOM APIではなくTL APIを使用する
	- 取得メソッドの優先順位
	  https://testing-library.com/docs/queries/about/
		 - getByRole  
		   `screen.getByRole(role, { name?: string })` 
		   ロールとアクセシブルネームで要素を取得  （最優先）
		- getByLabelText  
		  `screen.getByLabelText(text)` 
		  ラベルに紐づくフォーム要素を取得  
		- getByPlaceholderText  
		  `screen.getByPlaceholderText(text)` 
		  placeholder属性で要素を取得  
		- getByText  
		  `screen.getByText(text)` 
		  表示されているテキストで要素を取得  
		- getByDisplayValue  
		  `screen.getByDisplayValue(value)` 
		  フォームの表示値で要素を取得  
		- getByAltText  
		  `screen.getByAltText(text)` 
		  alt属性で画像要素を取得  
		- getByTitle  
		  `screen.getByTitle(text)` 
		  title属性で要素を取得  
		- getByTestId  
		  `screen.getByTestId(id)` 
		  data-testid属性で要素を取得（最終手段）
- Matcher
	結果を検証する際に使う。
	- ロジック検証時に使用するVitestMatcher
		- 等価比較  
			- `toBe(value)` プリミティブが等しい、または同一参照かを検証  
			- `toEqual(value)` 参照は異なるが構造が等しいかを検証  
		- 真偽値  
			- `toBeTruthy()` truthyかどうかを検証  
			- `toBeFalsy()` falsyかどうかを検証  
		- 数値  
			- `toBeGreaterThan(number)` より大きいか  
			- `toBeGreaterThanOrEqual(number)` 以上か  
			- `toBeLessThan(number)` より小さいか  
			- `toBeLessThanOrEqual(number)` 以下か  
			- `toBeCloseTo(number, precision?)` 小数点精度を指定して近似一致か  
		- 文字列  
			- `toContain(substring)` 含まれているか  
			- `toMatch(regexp)` 正規表現に一致するか  
			- `toHaveLength(length)` 文字列の長さが一致するか  
			- `expect.stringContaining(str)` 部分一致（オブジェクト検証用）  
			- `expect.stringMatching(regexp)` 正規表現一致（オブジェクト検証用）  
		- オブジェクト  
			- `toMatchObject(object)` 部分的に一致するか  
			- `toHaveProperty(key, value?)` 指定プロパティを持つか／値が一致するか  
			- `expect.objectContaining(object)` オブジェクトの部分一致を検証
	- コンポーネント検証時に使用するTLMatcher
	  コンポーネントのテストではユーザー操作の結果を確認するため、画面上での操作を再現し、実際の見え方を検証する
	  https://github.com/testing-library/jest-dom?tab=readme-ov-file#custom-matchers
		- 表示・存在  
			- `toBeInTheDocument()` DOMツリー内にその要素が存在しているか  
			- `toBeVisible()` ユーザーから視認可能か（CSS含む）  
		- テキスト・内容  
			- `toHaveTextContent(text)` 要素のテキストに指定文字列が含まれているか  
		- 属性  
			- `toHaveAttribute(name, value?)` 属性の存在／値が一致しているか  
		- クラス  
			- `toHaveClass(className)` 指定クラスが付与されているか  
		- フォーム関連  
			- `toHaveValue(value)` フォーム要素の値が一致しているか  
			- `toBeChecked()` チェックボックス／ラジオが選択状態か  
		- 状態（インタラクション）  
			- `toBeDisabled()` 要素が無効状態か  
			- `toBeEnabled()` 要素が有効状態か  
		- フォーカス  
			- `toHaveFocus()` フォーカスが当たっている要素か  
		- 構造  
			- `toContainElement(child)` 指定要素を子に持つか  
		- 空状態  
			- `toBeEmptyDOMElement()` 子ノードを持たないか
