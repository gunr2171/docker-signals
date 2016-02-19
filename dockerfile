FROM mono:latest

COPY source/ /tmp/
COPY ./runscript.sh /runscript.sh

RUN chmod +x /runscript.sh

RUN \
  xbuild /tmp/DockerSignalsExample.sln && \
  cp -r /tmp/DockerSignalsExample/bin/Debug/ /srv/program
  
ENTRYPOINT ["/runscript.sh"]