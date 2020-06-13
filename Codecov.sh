#!/usr/bin/env bash
curl -s https://codecov.io/bash > codecov
chmod +x codecov
./codecov -f "coverage.opencover.xml" -t $codecov_token