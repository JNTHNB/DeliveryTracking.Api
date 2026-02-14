#!/bin/sh

git filter-branch --env-filter '
OLD_EMAIL="jon.bosworth@valocityglobal.com"
CORRECT_NAME="Jon"
CORRECT_EMAIL="49577100+JNTHNB@users.noreply.github.com"
if [ "$GIT_COMMITTER_EMAIL" = "$OLD_EMAIL" ]
then
    export GIT_COMMITTER_NAME="$CORRECT_NAME"
    export GIT_COMMITTER_EMAIL="$CORRECT_EMAIL"
fi
if [ "$GIT_AUTHOR_EMAIL" = "$OLD_EMAIL" ]
then
    export GIT_AUTHOR_NAME="$CORRECT_NAME"
    export GIT_AUTHOR_EMAIL="$CORRECT_EMAIL"
    export GIT_AUTHOR_DATE="$GIT_AUTHOR_DATE"
fi
' --tag-name-filter cat -- --branches --tags
