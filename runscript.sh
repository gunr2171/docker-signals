#!/usr/bin/env bash
set -x

pid=0

# SIGUSR1-handler
my_handler() {
  echo "my_handler"
}

# SIGTERM-handler
term_handler() {
  if [ $pid -ne 0 ]; then
    echo "in term_handler"
    #kill -SIGINT "$pid"
	rm -f /shutdown.txt
	touch /shutdown.txt
    wait "$pid"
  fi
  exit 0; # 128 + 15 -- SIGTERM
}

# setup handlers
# on callback, kill the last background process, which is `tail -f /dev/null` and execute the specified handler
trap 'kill ${!}; my_handler' SIGUSR1
trap 'kill ${!}; term_handler' SIGTERM

# remove shutdown file if exists
rm -f /shutdown.txt

# run application
mono /srv/program/DockerSignalsExample.exe &
pid="$!"

# wait indefinetely
while true
do
  tail -f /dev/null & wait ${!}
done
