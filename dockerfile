FROM mono:latest

COPY source/ /tmp/

RUN \
  xbuild source/DockerSignalsExample.sln && /
  cp /tmp/source/DockerSignalsExample/bin/Debug/ /srv/program
  
ENTRYPOINT ["mono", "/srv/program/DockerSignalsExample.exe"]