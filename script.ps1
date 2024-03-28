get-childitem -Include bin -Recurse -force | Remove-Item -Force -Recurse
get-childitem -Include obj -Recurse -force | Remove-Item -Force -Recurse