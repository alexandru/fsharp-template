namespace Library.Tests

module Tests =
    open Expecto

    [<Tests>]
    let tests = testList "samples" [
        testCase "adding numbers" <| fun _ ->
            Expect.equal (Library.addNumbers 1 2) 3 "addNumbers"

        testCase "universe exists (╭ರᴥ•́)" <| fun _ ->
            let subject = true
            Expect.isTrue subject "I compute, therefore I am."

        testCase "I'm skipped (should skip)" <| fun _ ->
            Tests.skiptest "Yup, waiting for a sunny day..."

        testCase "contains things" <| fun _ ->
            Expect.containsAll
                [| 2; 3; 4 |] [| 2; 4 |]
                "This is the case; {2,3,4} contains {2,4}"

        testCase "Sometimes I want to ༼ノಠل͟ಠ༽ノ ︵ ┻━┻" <| fun _ ->
            Expect.equal "abcdëf" "abcdëf" "These should equal"

        test "I am (should fail)" {
            "╰〳 ಠ 益 ಠೃ 〵╯" |> Expect.equal true true
        }
    ]
