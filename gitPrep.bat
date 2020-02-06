set /P addChanges="Add Changes? (y/n): "
git pull
git status
if %addChanges%==y (goto :boi) else (goto :eof)

:boi
git add *
git commit -m "Prep Commit"
git push -u origin master